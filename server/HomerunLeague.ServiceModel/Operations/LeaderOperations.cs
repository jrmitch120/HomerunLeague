using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    [Route("/seasons/{year}/leaders", "GET")]
    public class GetLeadersRequest : PageableRequest, IReturn<GetLeadersResponse>
    {
        public int Year { get; set; }

        public int? TeamId { get; set; }
    }
}
