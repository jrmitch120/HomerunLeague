﻿using System.Collections.Generic;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel
{
    [Route("/{year}/divisions", "GET")]
    public class GetDivisions : PageableRequest, IReturn<GetDivisionsResponse> 
    {
        [ApiMember(IsRequired = true)]
        public int Year { get; set; }

        public bool IncludeInactive { get; set; }
    }

    public class GetDivisionsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }
        public List<Division> Divisions { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}

