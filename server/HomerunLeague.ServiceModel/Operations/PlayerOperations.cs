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
        [ApiMember(IsRequired = true)]
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
    public class CreatePlayer
    {
        public int MlbId { get; set; }

        public int MlbTeamId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public DateTime? BirthDate { get; set; }

        public int Weight { get; set; }

        public int HeightFeet { get; set; }

        public int HeightInches { get; set; }

        public int JerseyNumber { get; set; }

        public string Bats { get; set; }

        public string PrimaryPosition { get; set; }

        public string TeamName { get; set; }

        public bool Active { get; set; }
    }

    /***********************
    *    PUT OPERATIONS    *
    ***********************/
    [Route("/players/{id}", "PUT")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class PutPlayer : CreatePlayer
    {
        [ApiMember(IsRequired = true)]
        public int Id { get; set; }
    }
}
