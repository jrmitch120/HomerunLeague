using System;

namespace HomerunLeague.ServiceModel.ViewModels
{
    public class TeamListView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public bool Paid { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public TeamTotalsView Totals { get; set; }
    }
}
