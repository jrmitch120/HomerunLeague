using HomerunLeague.ServiceInterface;

namespace HomerunLeague.GameEngine
{
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