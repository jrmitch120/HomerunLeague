using System.Collections.Generic;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET RESPONSES     *
    ***********************/
    public class GetDivisionsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }

        public List<Division> Divisions { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
