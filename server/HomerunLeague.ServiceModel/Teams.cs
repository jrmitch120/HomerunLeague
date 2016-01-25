using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.ViewModels;
using ServiceStack;

namespace HomerunLeague.ServiceModel
{
    [Route("/teams/{id}", "GET", Summary = "Get a homerun league team")]
    public class GetTeam : IReturn<GetTeamsResponse> 
    {
        [ApiMember(Name = "Id", Description = "Id of the team.",
        ParameterType = "path", DataType = "string", IsRequired = true)]
        public int Id { get; set; }
    }

    public class GetTeamResponse : IHasResponseStatus 
    {
        public TeamView Team { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/teams", "GET", Summary = "Get a list of homerun league teams")]
    public class GetTeams : PageableRequest, IReturn<GetTeamsResponse> 
    {
        [ApiMember(Name = "Year", Description = "Filter teams by year.",
        ParameterType = "query", DataType = "string", IsRequired = false)]
        public int? Year { get; set; }
    }

    public class GetTeamsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }

        public List<TeamListView> Teams { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/teams", "POST", Summary = "Create a homerun league team.")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class CreateTeam : IReturn<GetTeamsResponse>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public List<int> PlayerIds { get; set; }
    }

    [Route("/teams/{id}/totals", "PUT", Summary = "Update a homerun league team's statistical totals.")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class UpdateTeamTotals
    {
        public int Id { get; set; }

        public TeamTotalsView TeamTotals { get; set; }
    }

    [Route("/teams/{id}", "DELETE", Summary = "Delete a homerun league team.")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class DeleteTeam
    {
        [ApiMember(IsRequired = true)]
        public int Id { get; set; }
    }
}

