using System.Collections.Generic;
using System.Net;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET OPERATIONS    *
    ***********************/

    [Route("/seasons/{year}/divisions/{id}", "GET")]
    [Route("/divisions/{id}", "GET")]
    public class GetDivision : PageableRequest, IReturn<GetDivisionsResponse>
    {
        [ApiMember(IsRequired = false)]
        public int? Year { get; set; }

        [ApiMember(IsRequired = true)]
        public int Id { get; set; }
    }

    [Route("/seasons/{year}/divisions", "GET")]
    [Route("/divisions", "GET")]
    public class GetDivisions : PageableRequest, IReturn<GetDivisionsResponse>
    {
        [ApiMember(IsRequired = false)]
        public int? Year { get; set; }

        [ApiMember(IsRequired = false)]
        public bool IncludeInactive { get; set; }
    }

    /***********************
    *   POST OPERATIONS    *
    ***********************/
    [Route("/seasons/{year}/divisions", "POST")]
    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
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

        public int Year { get; set; }

        public List<int> PlayerIds { get; set; }
    }

    /***********************
    *   DELETE OPERATIONS  *
    ***********************/
    [Route("/divisions/{id}", "Delete")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class DeleteDivision
    {
        public int Id { get; set; }
    }
}
