using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    // Game events carry meta about and action to be executed through the league engine.
    public class GameEvent
    {
        [AutoIncrement]
        public int Id { get; set; }

        public GameAction Action { get; set; }

        public object Options { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Completed { get; set; }
    }

    // Game actions drive game logic through the league engine.
    public enum GameAction
    {
        BioUpdate = 1,
        StatUpdate
    }

    public class BioUpdateOptions
    {
        public bool IncludeInactive { get; set; }
    }
    
}

