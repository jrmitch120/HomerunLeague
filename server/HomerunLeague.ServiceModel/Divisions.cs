using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel
{
    [Route("/divisions", "GET")]
    public class GetDivisions : PageableRequest, IReturn<GetDivisionsResponse> 
    {
        [ApiMember(IsRequired = false)]
        public int? Year { get; set; }

        [ApiMember(IsRequired = false)]
        public bool IncludeInactive { get; set; }
    }

    public class GetDivisionsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }

        public List<Division> Divisions { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/divisions", "POST")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class CreateDivisions
    {
        public CreateDivisions()
        {
            Divisions = new List<CreateDivision>();
        }

        public List<CreateDivision> Divisions { get; set; }
    }

    public class CreateDivision
    {
        public CreateDivision()
        {
            PlayerIds = new List<int>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PlayerRequirement { get; set; }

        public bool Active { get; set; }

        public int Order { get; set; }

        public List<int> PlayerIds { get; set; }
    }

    [Route("/divisions/{id}", "Delete")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class DeleteDivision
    {
        public int Id { get; set; }
    }
}

