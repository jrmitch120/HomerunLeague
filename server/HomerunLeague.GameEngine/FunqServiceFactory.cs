using Funq;
using HomerunLeague.ServiceInterface;

namespace HomerunLeague.GameEngine
{
    public class FunqServiceFactory : IServiceFactory
    {
        private readonly Container _container;
        
        public AdminServices CreateAdminServices()
        {
            return _container.Resolve<AdminServices>();
        }

        public DivisionServices CreateDivisionServices()
        {
            return _container.Resolve<DivisionServices>();
        }

        public PlayerServices CreatePlayerServices()
        {
            return _container.Resolve<PlayerServices>();
        }

        public StatServices CreateStatServices()
        {
            return _container.Resolve<StatServices>();
        }

        public TeamServices CreateTeamServices()
        {
            return _container.Resolve<TeamServices>();
        }

        public FunqServiceFactory(Container container)
        {
            _container = container;
        }
    }
}