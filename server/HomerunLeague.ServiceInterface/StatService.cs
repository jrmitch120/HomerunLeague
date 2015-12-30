using System.Net;
using HomerunLeague.ServiceInterface.RequestFilters;
using HomerunLeague.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    public class StatService : Service
    {
        [Secured]
        public HttpResult Put(PutGameLogs request)
        {
            request.GameLogs.ForEach(stat => stat.PlayerId = request.PlayerId);  // Associate stats with the player

            Db.SaveAll(request.GameLogs);
            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }

        [Secured]
        public HttpResult Put(PutSeasonTotals request)
        {
            request.SeasonTotals.PlayerId = request.PlayerId;

            Db.Save(request.SeasonTotals);
            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }
    }
}
