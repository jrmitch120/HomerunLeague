using System;
using System.Net;
using HomerunLeague.ServiceInterface.RequestFilters;
using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    [Secured]
    public class AdminServices : Service
    {
        public AdminServices()
        {

        }

        public GetLeagueEventsResponse Get(GetLeagueEvents request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<LeagueEvent>();

            if (!request.IncludeCompleted)
                query.And(q => q.Completed == null);

            query.OrderByDescending(q => q.Created);

            return new GetLeagueEventsResponse
            {
                LeagueEvents = Db.Select(query.PageTo(page)),
                Meta = new Meta(Request != null ? Request.AbsoluteUri : string.Empty) { Page = page, TotalCount = Db.Count(query) }
            };
        }

        public void Post(CreateLeagueEvent request)
        {

        }

        public HttpResult Put(UpdateLeagueEvent request)
        {
            int result = Db.Update(request.ConvertTo<LeagueEvent>());

            if (result == 0)
                throw new HttpError(HttpStatusCode.NotFound,
                    new ArgumentException("LeagueEvent {0} does not exist. ".Fmt(request.Id)));

            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }
    }
}

