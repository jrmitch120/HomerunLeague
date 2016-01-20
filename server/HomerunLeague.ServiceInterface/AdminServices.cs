﻿using System;
using System.Collections.Generic;
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
        public GetSettingsResponse Get(GetSettings request)
        {
            var settings = Db.Select<Setting>().ToDictionary(s => new KeyValuePair<string, string>(s.Name, s.Value));

            return new GetSettingsResponse
            {
                BaseballYear =
                    settings.ContainsKey("BaseballYear")
                        ? Convert.ToInt32(settings["BaseballYear"])
                        : DateTime.UtcNow.Month < 5 ? DateTime.UtcNow.Year - 1 : DateTime.UtcNow.Year,

                RegistrationOpen = settings.ContainsKey("RegistrationOpen")
                        ? Convert.ToBoolean(settings["RegistrationOpen"])
                        : DateTime.UtcNow.Month > 2 && DateTime.UtcNow.Month < 5
            };
        }

        public GetLeagueEventsResponse Get(GetLeagueEvents request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<LeagueEvent>();

            if (request.Status == EventStatus.Complete)
                query.And(q => q.Completed != null);
            else if (request.Status == EventStatus.Incomplete)
                query.And(q => q.Completed == null);

            if (request.Action != null)
                query.And(q => q.Action == request.Action);

            query.OrderByDescending(q => q.Created);

            return new GetLeagueEventsResponse
            {
                LeagueEvents = Db.Select(query.PageTo(page)),
                Meta = new Meta(Request != null ? Request.AbsoluteUri : string.Empty) { Page = page, TotalCount = Db.Count(query) }
            };
        }

        public HttpResult Post(CreateLeagueEvent request)
        {
            Db.Save(request.ConvertTo<LeagueEvent>());
            return new HttpResult { StatusCode = HttpStatusCode.Created };
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

