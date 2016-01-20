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

        public PlayerServices CreatePlayerServices()
        {
            return _container.Resolve<PlayerServices>();
        }

        public StatServices CreateStatService()
        {
            return _container.Resolve<StatServices>();
        }

        public FunqServiceFactory(Container container)
        {
            _container = container;
        }
    }
}