using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using ServiceStack;
using ServiceStack.OrmLite;

using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;


namespace HomerunLeague.ServiceInterface
{
    public class AdminServices : Service
    {
        public AdminServices()
        {

        }

        public GetGameEventsResponse Get(GetGameEvents request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<GameEvent>();

            if (!request.IncludeCompleted)
                query.And(q => q.Completed == null);

            query.OrderByDescending(q => q.Created);

            return new GetGameEventsResponse
            {
                GameEvents = Db.Select(query.PageTo(page)),
                Meta = new Meta(Request != null ? Request.AbsoluteUri : string.Empty) { Page = page, TotalCount = Db.Count(query) }
            };
        }

        public void Post(CreateGameEvent request)
        {

        }
    }
}

