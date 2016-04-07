using System.Collections.Generic;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET RESPONSES     *
    ***********************/
    public class GetLeadersResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }

        public List<Leader> Leaders { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    public class Leader
    {
        public int PlayerId { get; set; }

        public int DivisionId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        /* TODO
        public Uri PlayerImage { get; set; }
        public Uri PlayerImage2X { get; set; }

        public Uri TeamLogo { get; set; }
        public Uri TeamLogo2X { get; set; }
        */

        public string TeamName { get; set; }

        public int Hr { get; set; }
    }
}
