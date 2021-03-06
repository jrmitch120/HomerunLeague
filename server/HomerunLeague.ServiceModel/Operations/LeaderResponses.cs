﻿using System.Collections.Generic;
using HomerunLeague.ServiceModel.ViewModels;
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
}
