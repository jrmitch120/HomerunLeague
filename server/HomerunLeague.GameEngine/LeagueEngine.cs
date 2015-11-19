using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using HomerunLeague.GameEngine.Bios;
using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.GameEngine
{
    public class LeagueEngine
    {
        private readonly Dictionary<GameAction, Action<object>> _gameActions; 

        private readonly IBioData _bioData;
        private readonly Services _services;

        private readonly Timer _timer;
        private volatile bool _processing;

        public LeagueEngine(IBioData bioData, Services services)
        {
            _bioData = bioData;
            _services = services;

            _timer = new Timer(TimeSpan.FromSeconds(15).TotalMilliseconds);
            _timer.Elapsed += Run;
            _timer.AutoReset = true;

            _gameActions = new Dictionary<GameAction, Action<object>>
            {
                {GameAction.BioUpdate, o => UpdateBio(o.ConvertTo<BioUpdateOptions>())}
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
            if (_processing)
                return;

            _processing = true;

            GetGameEventsResponse response;
            
            int page = 0;

            do
            {
                page++;
                response = _services.AdminSvc.Get(new GetGameEvents{Page = page});
                response.GameEvents.ForEach(ProcessRequest);

            } while (response.Meta.Page < response.Meta.TotalPages);

            _processing = false;
        }

        private void ProcessRequest(GameEvent gameEvent)
        {
            _gameActions[gameEvent.Action].Invoke(gameEvent.Options);

            gameEvent.Completed = DateTime.Now;
            
            // TODO: update
        }

        private void UpdateBio(BioUpdateOptions options)
        {
            GetPlayersResponse response;
            int page = 0;

            do
            {
                page++;
                response =
                    _services.PlayerSvc.Get(new GetPlayers
                    {
                        Page = page,
                        IncludeInactive = options.IncludeInactive
                    });

                if (response.Players.Any())
                {
                    _bioData.Update(response.Players);
                    _services.PlayerSvc.Put(new PutPlayers {Players = response.Players});
                }
            } while (response.Meta.Page < response.Meta.TotalPages);
        }
    }
}

