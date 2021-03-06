﻿using System;
using System.Collections.Generic;
using HomerunLeague.ServiceModel.Operations;

namespace HomerunLeague.ServiceModel.ViewModels
{
    public class TeamView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public bool Paid { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public TeamTotalsView Totals { get; set; }

        public List<Leader> TeamLeaders { get; set; }
    }
}