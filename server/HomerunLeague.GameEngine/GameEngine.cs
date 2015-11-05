using System;
using System.Collections.Generic;

using HomerunLeague.ServiceInterface;
using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.GameEngine
{
    public class GameEngine
    {
        private readonly IPlayerData _playerData;

        private readonly Services _services;

        public GameEngine(IPlayerData playerData, Services services)
        {
            _playerData = playerData;
            _services = services;
        }

        public void Start()
        {
            // TODO
        }

        public void Stop()
        {
            // TODO
        }

        private void Run()
        {
            // Todo
        }

        private void GetPendingRequests()
        {
            // TODO
        }

        private void ProcessRequest(ProcessingRequest request)
        {
            switch (request.Type)
            {
                case ProcessingRequestType.BioUpdate:
                    _playerData.UpdatePlayers(_services.PlayerSvc.Get(new GetPlayers{Active = true}).Players); // Got to fetchall
                    break;
            }
        }
    }

    public interface IPlayerData 
    {
        void UpdatePlayers(IEnumerable<Player> players);
    }

    public class MlbPlayerData : IPlayerData
    {
        public void UpdatePlayers(IEnumerable<Player> players)
        {
            
        }
    }

    public class Services
    {
        public readonly AdminServices AdminSvc;
        public readonly PlayerServices PlayerSvc;

        public Services(AdminServices adminSvc, PlayerServices playerSvc)
        {
            AdminSvc = adminSvc;
            PlayerSvc = playerSvc;
        }
    }
}

