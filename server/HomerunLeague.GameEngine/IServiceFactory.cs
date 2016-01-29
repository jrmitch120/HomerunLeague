using HomerunLeague.ServiceInterface;

namespace HomerunLeague.GameEngine
{
    public interface IServiceFactory
    {
        AdminServices CreateAdminServices();
        DivisionServices CreateDivisionServices();
        PlayerServices CreatePlayerServices();
        StatServices CreateStatServices();
        TeamServices CreateTeamServices();
    }
}