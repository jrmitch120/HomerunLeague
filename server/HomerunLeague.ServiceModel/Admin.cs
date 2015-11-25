using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel
{
    [Route("/game/events", "GET")]
    public class GetLeagueEvents : PageableRequest, IReturn<GetLeagueEventsResponse>
    {
        public bool IncludeCompleted { get; set; } 
    }

    public class GetLeagueEventsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }
        public List<LeagueEvent> LeagueEvents { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/game/events", "POST")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class CreateLeagueEvent : LeagueEvent
    {
        
    }
    
    [Route("/game/events", "PUT")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class UpdateLeagueEvent : LeagueEvent
    {

    }
}


