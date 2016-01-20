using HomerunLeague.ServiceInterface;

namespace HomerunLeague.GameEngine
{
    public interface IServiceFactory
    {
        AdminServices CreateAdminServices();
        PlayerServices CreatePlayerServices();
        StatServices CreateStatService();
    }
}