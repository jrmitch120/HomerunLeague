using System;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    [Route("/seasons/{year}/recent", "GET")]
    public class GetRecentHrRequest : PageableRequest, IReturn<GetRecentHrResponse>
    {
        [ApiMember(Name = "Year", Description = "Year of the league to get.", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Year { get; set; }

        [ApiMember(Name = "Start", Description = "Get a list of recent hr's starting from this date.", ParameterType = "query", DataType = "DateTime", IsRequired = false)]
        public DateTime? Start { get; set; }
    }
}
