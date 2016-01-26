using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET OPERATIONS    *
    ***********************/
    [Route("/players/{id}", "GET")]
    public class GetPlayer : IReturn<GetPlayerResponse>
    {
        public int Id { get; set; }
    }

    [Route("/players", "GET")]
    public class GetPlayers : PageableRequest, IReturn<GetPlayersResponse>
    {
        public bool IncludeInactive { get; set; }
    }

    /***********************
    *   POST OPERATIONS    *
    ***********************/
    [Route("/players", "POST")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class CreatePlayers
    {
        [ApiMember(IsRequired = true)]
        public List<Player> Players { get; set; }
    }

    /***********************
    *    PUT OPERATIONS    *
    ***********************/
    [Route("/players", "PUT")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class PutPlayers
    {
        [ApiMember(IsRequired = true)]
        public List<Player> Players { get; set; }
    }
}
