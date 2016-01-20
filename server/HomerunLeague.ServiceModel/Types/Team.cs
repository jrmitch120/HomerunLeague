using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    public class Team : IAudit
    {
        public Team()
        {
            Players = new List<Player>();
            Totals = new TeamTotals();
        }

        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public string Email { get; set; }

        public bool Validated { get; set; }

        public string ValidationToken { get; set; }

        public bool Paid { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        [Reference]
        public TeamTotals Totals { get; set; }

        [Ignore]
        public List<Player> Players { get; set; }
    }
}

