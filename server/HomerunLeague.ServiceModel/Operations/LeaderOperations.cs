using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    [Route("/seasons/{year}/leaders", "GET")]
    public class GetLeadersRequest : PageableRequest, IReturn<GetLeadersResponse>
    {
        public int Year { get; set; }

        public int? TeamId { get; set; }
    }

    [Route("/seasons/{year}/leaders/{month}/{day}", "GET")]
    public class GetLeadersByDateRequest : PageableRequest, IReturn<GetLeadersResponse>
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int? Day { get; set; }
    }
}
