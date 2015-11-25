using HomerunLeague.ServiceInterface;

namespace HomerunLeague.GameEngine
{
    public class Services
    {
        public readonly AdminServices AdminSvc;
        public readonly PlayerServices PlayerSvc;
        public readonly StatService StatSvc;

        public Services(AdminServices adminSvc, PlayerServices playerSvc, StatService statSvc)
        {
            AdminSvc = adminSvc;
            PlayerSvc = playerSvc;
            StatSvc = statSvc;
        }
    }
}