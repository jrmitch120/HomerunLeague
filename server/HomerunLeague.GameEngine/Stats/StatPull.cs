using System.Collections.Generic;
using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.GameEngine.Stats
{
    public class StatPull
    {
        public int PlayerId { get; set; }

        public int Year { get; set; }

        public PlayerTotals Totals { get; set; }

        public List<GameLog> GameLogs { get; set; }

        public StatPull()
        {
            GameLogs = new List<GameLog>();
        }
    }
}
