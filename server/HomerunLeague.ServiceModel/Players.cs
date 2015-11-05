using System;
using System.Collections.Generic;
using System.Net;
using ServiceStack;
using HomerunLeague.ServiceModel.Types;

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
        [ApiMember(IsRequired = false)]
        public bool? Active { get; set; }
    }

    public class GetPlayersResponse : IHasResponseStatus 
    {
        public Meta Meta { get; set; }
        public List<Player> Players { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}

