using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.Types;
using HomerunLeague.ServiceModel.ViewModels;
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
        public TeamView Team { get; set; }

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

        public List<TeamView> Teams { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/{year}/teams", "POST")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class CreateTeam : IReturn<GetTeamsResponse>
    {
        public string Name { get; set; }

        public int Year { get; set; }

        public string Email { get; set; }

        public List<int> PlayerIds { get; set; }
    }
}

