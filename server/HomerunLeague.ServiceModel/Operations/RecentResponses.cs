using System.Collections.Generic;
using HomerunLeague.ServiceModel.ViewModels;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET RESPONSES     *
    ***********************/
    public class GetRecentHrResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }

        public List<RecentHr> RecentHrs { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
