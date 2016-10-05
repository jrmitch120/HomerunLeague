using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using HomerunLeague.GameEngine.Bios;
using HomerunLeague.GameEngine.Stats;
using HomerunLeague.ServiceInterface;
using HomerunLeague.ServiceModel.Operations;
using HomerunLeague.ServiceModel.Types;
using HomerunLeague.ServiceModel.ViewModels;
using ServiceStack;

namespace HomerunLeague.GameEngine 
{
    public class LeagueEngine : IDisposable
    {
        private readonly Dictionary<LeagueAction, Action<object>> _gameActions; 

        private readonly IBioData _bioData;
        private readonly IStatData _statData;

        private readonly Services _services;

        private GetSettingsResponse _settings;

        private readonly Timer _timer;
        private volatile bool _running;
        
        public LeagueEngine(IBioData bioData, IStatData statData, Services services)
        {
            _bioData = bioData;
            _statData = statData;
            _services = services;
            
            _timer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            _timer.Elapsed += Run;
            _timer.AutoReset = true;

            _gameActions = new Dictionary<LeagueAction, Action<object>>
            {
                {LeagueAction.BioUpdate, o => UpdatePlayerBios(o?.ConvertTo<BioUpdateOptions>())},
                {LeagueAction.StatUpdate, o => UpdatePlayerStats(o?.ConvertTo<StatUpdateOptions>())},
                {LeagueAction.TeamUpdate, o => UpdateTeams(o?.ConvertTo<TeamUpdateOptions>())}
            };
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void Run(object sender, ElapsedEventArgs e)
        {
            if (_running) return;

            try
            {
                _running = true;

                _settings = _services.AdminSvc.Get(new GetSettings());

                HeartBeat();

                GetLeagueEventsResponse response;

                int page = 0;

                do
                {
                    page++;
                    response = _services.AdminSvc.Get(new GetLeagueEvents {Status = EventStatus.Incomplete, Page = page});
                    response.LeagueEvents.ForEach(ProcessRequest);

                } while (response.Meta.Page < response.Meta.TotalPages);
            }
            catch (Exception ex)
            {

                // TODO, excetion
            }
            finally
            {
                _running = false;
                _services.Dispose();
            }
        }

        private void ProcessRequest(LeagueEvent leagueEvent)
        {
            if (leagueEvent.Completed != null)
                return;

            _gameActions[leagueEvent.Action].Invoke(leagueEvent.Options);

            leagueEvent.Completed = DateTime.UtcNow;

            _services.AdminSvc.Put(leagueEvent.ConvertTo<UpdateLeagueEvent>());
        }

        private void HeartBeat()
        {
            // Automatic stat updates once every 12 hrs
            var lastStat =
                _services.AdminSvc.Get(new GetLeagueEvents
                {
                    Action = LeagueAction.StatUpdate,
                }).LeagueEvents.FirstOrDefault();

            if (lastStat == null || DateTime.UtcNow.AddHours(-12) > lastStat.Completed)
                _services.AdminSvc.Post(new CreateLeagueEvent
                {
                    Action = LeagueAction.StatUpdate,
                    Options = new StatUpdateOptions {Year = _settings.BaseballYear}
                });

            // Automatic bio updates once a week
            var lastBio =
                _services.AdminSvc.Get(new GetLeagueEvents
                {
                    Action = LeagueAction.BioUpdate,
                }).LeagueEvents.FirstOrDefault();

            if (lastBio == null || DateTime.UtcNow.AddDays(-7) > lastBio.Completed)
                _services.AdminSvc.Post(new CreateLeagueEvent
                {
                    Action = LeagueAction.BioUpdate
                });
        }

        /// <summary>
        /// Update players' biographical information from the IBioData provider
        /// </summary>
        /// <param name="options">Bio update options</param>
        private void UpdatePlayerBios(BioUpdateOptions options)
        {
            options = options ?? new BioUpdateOptions();

            _services.PlayerSvc.BatchPlayerAction(new GetPlayers { IncludeInactive = options.IncludeInactive }, players =>
            {
                _bioData.UpdatePlayerBios(players);

                var updateRequests = new List<PutPlayer>();
                players.ForEach(p => updateRequests.Add(p.ConvertTo<PutPlayer>()));

                _services.PlayerSvc.Put(updateRequests.ToArray());
            });
        }

        /// <summary>
        /// Update players' statistics from the IStatData provider
        /// </summary>
        /// <param name="options">Stat update options</param>
        private void UpdatePlayerStats(StatUpdateOptions options)
        {
            options = options ?? new StatUpdateOptions {Year = _settings.BaseballYear};

            _services.PlayerSvc.BatchPlayerAction(new GetPlayers(), players =>
            {
                foreach (var player in players)
                {
                    var stats = _statData.FetchStats(player, options.Year);

                    if (stats.GameLogs.Any()) // Did we find any stats?
                    {
                        _services.StatSvc.Put(stats.GameLogs.Map(gl => gl.ConvertTo<PutGameLog>()).ToArray());
                        _services.StatSvc.Put(stats.Totals.ConvertTo<PutPlayerTotals>());
                    }
                }
            });

            // Since we've update statistics, create a new request to update teams.
            _services.AdminSvc.Post(new CreateLeagueEvent
            {
                Action = LeagueAction.TeamUpdate,
                Options = new TeamUpdateOptions {Year = options.Year}
            });
        }

        private void UpdateTeams(TeamUpdateOptions options)
        {
            options = options ?? new TeamUpdateOptions {Year = _settings.BaseballYear };

            // TODO: var players = new Dictionary<int, Player>();

            _services.TeamSvc.BatchTeamAction(new GetTeams { Year = options.Year}, teams =>
            {
                foreach (var team in teams)
                {
                    var fullTeam = _services.TeamSvc.Get(new GetTeam {Id = team.Id}).Team;
                    var originalHr = team.Totals.Hr;

                    team.Totals.Hr = 0;

                    foreach (var player in fullTeam.TeamLeaders)
                    {
                        // Get play totals for the year.  If he doesn't have any yet, just get an empty stat object.
                        var playerTotal = _services.PlayerSvc.Get(new GetPlayer {Id = player.PlayerId})
                            .Player.PlayerTotals.FirstOrDefault(total => total.Year == options.Year) ?? new PlayerTotals();

                        team.Totals.Hr += playerTotal.Hr;
                        team.Totals.Ab += playerTotal.Ab;
                    }

                    team.Totals.HrMovement = team.Totals.Hr - originalHr;

                    _services.TeamSvc.Put(new UpdateTeamTotals {Id = team.Id, TeamTotals = team.Totals});
                }
            });
        }

        public void Dispose()
        {
            _services?.Dispose();
        }
    }

    public static class ServiceExtensions
    {
        public static void BatchPlayerAction(this PlayerServices svc, GetPlayers request, Action<List<Player>> process)
        {
            GetPlayersResponse response;
            int page = 0;

            do
            {
                page++;
                request.Page = page;
                response = svc.Get(request);

                if (response.Players.Any())
                    process.Invoke(response.Players);

            } while (response.Meta.Page < response.Meta.TotalPages);
        }

        public static void BatchTeamAction(this TeamServices svc, GetTeams request, Action<List<TeamListView>> process)
        {
            GetTeamsResponse response;
            int page = 0;

            do
            {
                page++;
                request.Page = page;
                response = svc.Get(request);

                if (response.Teams.Any())
                    process.Invoke(response.Teams);

            } while (response.Meta.Page < response.Meta.TotalPages);
        }
    }
}

