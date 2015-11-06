using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel
{
    [Route("/players/{id}", "GET")]
    public class GetPlayer : IReturn<GetPlayerResponse>
    {
        public int Id { get; set; }
    }

    public class GetPlayerResponse : IHasResponseStatus 
    {
        public Player Player { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/players", "GET")]
    public class GetPlayers : PageableRequest, IReturn<GetPlayersResponse>
    {
        public bool IncludeInactive { get; set; }
    }

    public class GetPlayersResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }
        public List<Player> Players { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/players", "PUT")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    public class PutPlayers 
    {
        [ApiMember(IsRequired = true)]
        public List<Player> Players { get; set; }
    }
}

