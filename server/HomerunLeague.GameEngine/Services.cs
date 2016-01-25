using System;
using HomerunLeague.ServiceInterface;

namespace HomerunLeague.GameEngine
{
    /// <summary>
    /// Container for services that are required by the league engine to make the ship run.
    /// </summary>
    public class Services : IDisposable
    {
        private readonly IServiceFactory _factory;

        // Using lazy here because all services may not be required at any given time
        private Lazy<AdminServices> _adminServices;
        private Lazy<PlayerServices> _playerServices;
        private Lazy<StatServices> _statServices;
        private Lazy<TeamServices> _teamServices;

        public AdminServices AdminSvc => _adminServices.Value;
        public PlayerServices PlayerSvc => _playerServices.Value;
        public StatServices StatSvc => _statServices.Value;
        public TeamServices TeamSvc => _teamServices.Value;

        public Services(IServiceFactory factory)
        {
            _factory = factory;

            Init();
        }

        private void Init()
        {
            _adminServices = new Lazy<AdminServices>(_factory.CreateAdminServices);
            _playerServices = new Lazy<PlayerServices>(_factory.CreatePlayerServices);
            _statServices = new Lazy<StatServices>(_factory.CreateStatServices);
            _teamServices = new Lazy<TeamServices>(_factory.CreateTeamServices);
        }

        public void Dispose()
        {
            // We want to impliment dispose to clean up the services that operate using database connections
            if (_adminServices.IsValueCreated)
                _adminServices.Value.Dispose();
            if (_playerServices.IsValueCreated)
                _playerServices.Value.Dispose();
            if (_statServices.IsValueCreated)
                _statServices.Value.Dispose();
            if (_teamServices.IsValueCreated)
                _teamServices.Value.Dispose();

            Init();
        }
    }
}
