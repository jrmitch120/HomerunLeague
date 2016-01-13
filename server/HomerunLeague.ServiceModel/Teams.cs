using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel
{
    [Route("/{year}/teams/{id}", "GET")]
    public class GetTeam : IReturn<GetTeamsResponse> 
    {
        [ApiMember(IsRequired = true)]
        public int Year { get; set; }

        [ApiMember(IsRequired = true)]
        public int Id { get; set; }
    }

    public class GetTeamResponse : IHasResponseStatus 
    {
        public Team Team { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/{year}/teams", "GET")]
    public class GetTeams : PageableRequest, IReturn<GetTeamsResponse> 
    {
        [ApiMember(IsRequired = true)]
        public int Year { get; set; }
    }

    public class GetTeamsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }
        public List<Team> Teams { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/{year}/teams", "POST")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class CreateTeam : Team, IReturn<GetTeamsResponse>
    {

    }
}

