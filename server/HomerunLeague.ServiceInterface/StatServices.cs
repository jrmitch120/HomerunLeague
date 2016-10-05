using System;
using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceInterface.Extensions;
using HomerunLeague.ServiceInterface.RequestFilters;
using HomerunLeague.ServiceModel.Operations;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    [Secured(ApplyTo.Post | ApplyTo.Put | ApplyTo.Delete)]
    public class StatServices : Service
    {
        // Get GameLogs from collection
        public GetGameLogsResponse Get(GetGameLogs request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<GameLog>().Where(q => q.GameDate >= new DateTime(request.Year, 1, 1));

            if (request.Start.HasValue)
                query.And(q => q.GameDate >= request.Start);

            query.OrderByDescending(q => q.GameDate).ThenBy(q => q.PlayerId);

            return new GetGameLogsResponse
            {
                GameLogs = Db.Select(query.PageTo(page)),
                Meta = new Meta(Request?.AbsoluteUri) { Page = page, TotalCount = Db.Count(query) }
            };
        }

        // Get GameLogs for player
        public GetGameLogsResponse Get(GetGameLogsForPlayer request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<GameLog>().Where(g => g.PlayerId == request.PlayerId);

            if (request.Year.HasValue)
            {
                query.And(q => q.GameDate >= new DateTime(request.Year.Value, 1, 1));
                query.And(q => q.GameDate <= new DateTime(request.Year.Value, 12, 31));
            }

            query.OrderByDescending(q => q.GameDate);

            return new GetGameLogsResponse
            {
                GameLogs = Db.Select(query.PageTo(page)),
                Meta = new Meta(Request?.AbsoluteUri) { Page = page, TotalCount = Db.Count(query) }
            };
        }

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
