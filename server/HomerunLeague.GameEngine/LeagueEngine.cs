using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using HomerunLeague.GameEngine.Bios;
using HomerunLeague.GameEngine.Stats;
using HomerunLeague.ServiceInterface;
using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.GameEngine
{
    public class LeagueEngine
    {
        private readonly Dictionary<LeagueAction, Action<object>> _gameActions; 

        private readonly IBioData _bioData;
        private readonly IStatData _statData;
        private readonly Services _services;

        private readonly Timer _timer;
        private volatile bool _processing;

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
                {LeagueAction.BioUpdate, o => UpdateBio(o.ConvertTo<BioUpdateOptions>())},
                {LeagueAction.StatUpdate, o => UpdateStats(o.ConvertTo<StatUpdateOptions>())}
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
            try
            {
                if (_processing)
                    return;

                _processing = true;

                GetLeagueEventsResponse response;

                int page = 0;

                do
                {
                    page++;
                    response = _services.AdminSvc.Get(new GetLeagueEvents {Page = page});
                    response.LeagueEvents.OrderBy(le => le.Created).ToList().ForEach(ProcessRequest);

                } while (response.Meta.Page < response.Meta.TotalPages);
            }
            catch (Exception ex)
            {

                // TODO, excetion
            }
            finally
            {
                _processing = false;
            }
        }

        private void ProcessRequest(LeagueEvent leagueEvent)
        {
            if (leagueEvent.Completed != null)
                return;

            _gameActions[leagueEvent.Action].Invoke(leagueEvent.Options);

            leagueEvent.Completed = DateTime.Now;

            _services.AdminSvc.Put(leagueEvent.ConvertTo<UpdateLeagueEvent>());
        }

        /// <summary>
        /// Update players' biographical information from the IBioData provider
        /// </summary>
        /// <param name="options">Bio update options</param>
        private void UpdateBio(BioUpdateOptions options)
        {
            _services.PlayerSvc.BatchPlayerAction(new GetPlayers { IncludeInactive = options.IncludeInactive }, players =>
            {
                _bioData.UpdatePlayerBios(players);
                _services.PlayerSvc.Put(new PutPlayers { Players = players });
            });
        }

        /// <summary>
        /// Update players' statistics from the IStatData provider
        /// </summary>
        /// <param name="options">Stat update options</param>
        private void UpdateStats(StatUpdateOptions options)
        {
            int year = DateTime.Now.Month < 5 ? DateTime.Now.Year - 1 : DateTime.Now.Year;

            _services.PlayerSvc.BatchPlayerAction(new GetPlayers(), players =>
            {
                foreach (var player in players)
                {
                    var stats = _statData.FetchStats(player, year);

                    _services.StatSvc.Put(new PutGameLogs
                    {
                        PlayerId = player.Id,
                        GameLogs = stats.GameLogs
                    });

                    _services.StatSvc.Put(new PutSeasonTotals
                    {
                        PlayerId = player.Id,
                        SeasonTotals = stats.SeasonTotals
                    });
                }
            });
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

                if(response.Players.Any())
                    process.Invoke(response.Players);

            } while (response.Meta.Page < response.Meta.TotalPages);
        }
    }
}

