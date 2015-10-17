using System;
using System.Collections.Generic;

using ServiceStack;

using HomerunLeague.ServiceModel.Types;
using HomerunLeague.ServiceModel;


namespace HomerunLeague.ServiceModel
{
    [Route("/{year}/divisions")]
    public class GetDivisions : IReturn<GetDivisionsResponse> 
    {
        public int Year { get; set; }
        public int? Page { get; set; }
    }

    public class GetDivisionsResponse : IHasResponseStatus 
    {
        public Paging Paging { get; set; }
        public List<Division> Divisions { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}

