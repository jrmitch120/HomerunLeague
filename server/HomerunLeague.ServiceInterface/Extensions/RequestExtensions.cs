using System.Collections.Generic;
using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Operations;
using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.ServiceInterface.Extensions
{
    public static class RequestExtensions
    {
        public static List<Division> GetAll(this DivisionServices service, GetDivisions request)
        {
            var seasons = new List<Division>();
            GetDivisionsResponse response;

            request.Page = 1;

            do
            {
                response = service.Get(request);
                seasons.AddRange(response.Divisions);

                request.Page++;
            } while (request.Page <= response.Meta.TotalPages);

            return seasons;
        }

        public static List<Player> GetAll(this PlayerServices service, GetPlayers request)
        {
            var seasons = new List<Player>();
            GetPlayersResponse response;

            request.Page = 1;

            do
            {
                response = service.Get(request);
                seasons.AddRange(response.Players);

                request.Page++;
            } while (request.Page <= response.Meta.TotalPages);

            return seasons;
        }
    }
}
