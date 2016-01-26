using System.Net;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET OPERATIONS    *
    ***********************/
    [Route("/admin/settings", "GET")]
    public class GetSettings { }

    [Route("/admin/events", "GET")]
    public class GetLeagueEvents : PageableRequest, IReturn<GetLeagueEventsResponse>
    {
        public EventStatus? Status { get; set; }

        public LeagueAction? Action { get; set; }
    }

    /***********************
    *   POST OPERATIONS    *
    ***********************/
    [Route("/admin/events", "POST")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class CreateLeagueEvent
    {
        public LeagueAction Action { get; set; }

        public object Options { get; set; }
    }

    /***********************
    *    PUT OPERATIONS    *
    ***********************/
    [Route("/admin/events/{Id}", "PUT")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class UpdateLeagueEvent : LeagueEvent
    {

    }
}
