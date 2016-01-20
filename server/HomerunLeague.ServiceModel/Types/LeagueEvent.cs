using System;
using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    // League events carry meta about and action to be executed through the league engine.
    public class LeagueEvent : IAudit
    {
        [AutoIncrement]
        public int Id { get; set; }

        public LeagueAction Action { get; set; }

        public object Options { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public DateTime? Completed { get; set; }
    }

    // League actions drive game logic through the league engine.
    public enum LeagueAction
    {
        BioUpdate = 1,
        StatUpdate
    }

    public class BioUpdateOptions
    {
        public bool IncludeInactive { get; set; }
    }

    public class StatUpdateOptions
    {
        public int Year { get; set; }
    }
}

