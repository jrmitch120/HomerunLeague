﻿using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.ViewModels;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET OPERATIONS    *
    ***********************/
    [Route("/teams/{id}", "GET", Summary = "Get a homerun league team")]
    [Route("/seasons/{year}/teams/{id}", "GET")]
    public class GetTeam : IReturn<GetTeamsResponse>
    {
        [ApiMember(Name = "Id", Description = "Id of the team.",
        ParameterType = "path", DataType = "string", IsRequired = true)]
        public int Id { get; set; }

        [ApiMember(Name = "Year", Description = "Year that the team is enrolled in.",
        ParameterType = "path", DataType = "string", IsRequired = false)]
        public int? Year { get; set; }
    }

    [Route("/seasons/{year}/teams", "GET", Summary = "Get a list of homerun league teams for a season")]
    public class GetTeams : PageableRequest, IReturn<GetTeamsResponse>
    {
        [ApiMember(Name = "Year", Description = "Filter teams by year.",
        ParameterType = "path", DataType = "string", IsRequired = true)]
        public int Year { get; set; }

        [ApiMember(Name = "Year", Description = "Filter teams by name.",
             ParameterType = "query", DataType = "string", IsRequired = false)]
        public string Name { get; set; }
    }

    /***********************
    *   POST OPERATIONS    *
    ***********************/
    [Route("/seasons/{year}/teams", "POST", Summary = "Create a homerun league team.")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class CreateTeam : IReturn<GetTeamsResponse>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int Year { get; set; }

        public List<int> PlayerIds { get; set; }
    }

    /***********************
    *    PUT OPERATIONS    *
    ***********************/
    [Route("/teams/{id}/totals", "PUT", Summary = "Update a homerun league team's statistical totals.")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class UpdateTeamTotals
    {
        public int Id { get; set; }

        public TeamTotalsView TeamTotals { get; set; }
    }

    /***********************
    *   DELETE OPERATIONS  *
    ***********************/
    [Route("/teams/{id}", "DELETE", Summary = "Delete a homerun league team.")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class DeleteTeam
    {
        [ApiMember(IsRequired = true)]
        public int Id { get; set; }
    }
}
