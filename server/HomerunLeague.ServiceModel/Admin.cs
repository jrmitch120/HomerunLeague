using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel
{
    [Route("/game/events", "GET")]
    public class GetGameEvents : PageableRequest, IReturn<GetGameEventsResponse>
    {
        public bool IncludeCompleted { get; set; } 
    }

    public class GetGameEventsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }
        public List<GameEvent> GameEvents { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/game/events", "POST")]
    public class CreateGameEvent : GameEvent, IReturn<CreateActionResponse>
    {
        
    }

    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class CreateActionResponse
    {

    }
}


