using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel
{
    [Route("/admin/settings")]
    public class GetSettings { }

    public class GetSettingsResponse
    {
        public int BaseballYear { get; set; }

        public bool RegistrationOpen { get; set; }
    }

    [Route("/admin/events", "GET")]
    public class GetLeagueEvents : PageableRequest, IReturn<GetLeagueEventsResponse>
    {
        public EventStatus? Status { get; set; }

        public LeagueAction? Action { get; set; }
    }

    public enum EventStatus
    {
        Complete,
        Incomplete,
    }

    public class GetLeagueEventsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }

        public List<LeagueEvent> LeagueEvents { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/admin/events", "POST")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class CreateLeagueEvent
    {
        public LeagueAction Action { get; set; }

        public object Options { get; set; }
    }
    
    [Route("/admin/events/{Id}", "PUT")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class UpdateLeagueEvent : LeagueEvent
    {

    }
}


