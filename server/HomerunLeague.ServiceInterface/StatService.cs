using System.Net;
using System.Linq;
using HomerunLeague.ServiceInterface.RequestFilters;
using HomerunLeague.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    public class StatService : Service
    {
        //public GetPlayerResponse Get(GetPlayer request)
        //{
        //    var player = Db.LoadSingleById<Player>(request.Id);

        //    if (player == null)
        //        throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("PlayerId {0} does not exist. ".Fmt(request.Id)));

        //    return new GetPlayerResponse { Player = player };
        //}

        //public GetPlayersResponse Get(GetPlayers request)
        //{
        //    int page = request.Page ?? 1;

        //    var query = Db.From<Player>();

        //    if (!request.IncludeInactive)
        //        query.And(q => q.Active);

        //    return new GetPlayersResponse
        //    {
        //        Players = Db.Select(query.PageTo(page)),
        //        Meta = new Meta(Request != null ? Request.AbsoluteUri : string.Empty) { Page = page, TotalCount = Db.Count(query) }
        //    };
        //}

        [Secured]
        public HttpResult Put(PutGameLogs request)
        {
            var debug =
                request.GameLogs.GroupBy(n => n.Id)
                    .Select(n => new {Id = n.Key, Count = n.Count()})
                    .OrderByDescending(n => n.Count);

            request.GameLogs.ForEach(stat => stat.PlayerId = request.PlayerId);

            // TESTING
            //foreach (var log in request.GameLogs)
            //    Db.Save(log);

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
