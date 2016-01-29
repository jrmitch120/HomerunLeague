using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceInterface.RequestFilters;
using HomerunLeague.ServiceModel.Operations;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    [Secured]
    public class StatServices : Service
    {
        // Update GameLog
        public HttpResult Put(PutGameLog request)
        {
            var gameLog = request.ConvertTo<GameLog>();

            Db.Save(gameLog);

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }

        // Batch Update GameLog
        public List<HttpResult> Put(PutGameLog[] requests)
        {
            using (var trans = Db.OpenTransaction())
            {
                var responses = requests.Map(Put);

                trans.Commit();
                return responses;
            }
        }

        // Update PlayerTotals
        public HttpResult Put(PutPlayerTotals request)
        {
            var totals = request.ConvertTo<PlayerTotals>();

            Db.Save(totals);

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }

        // Batch Update PlayerTotals
        public List<HttpResult> Put(PutPlayerTotals[] requests)
        {
            using (var trans = Db.OpenTransaction())
            {
                var responses = requests.Map(Put);

                trans.Commit();
                return responses;
            }
        }
    }
}
