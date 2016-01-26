using System.Collections.Generic;
using HomerunLeague.ServiceModel.ViewModels;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET RESPONSES     *
    ***********************/
    public class GetTeamResponse : IHasResponseStatus
    {
        public TeamView Team { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    public class GetTeamsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }

        public List<TeamListView> Teams { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
