using System.Collections.Generic;

namespace HomerunLeague.ServiceModel.Types
{
    public class PlayerStats
    {
        public int PlayerId { get; set; }

        public int Year { get; set; }

        public SeasonTotals SeasonTotals { get; set; }

        public List<GameLog> GameLogs { get; set; }

        public PlayerStats()
        {
            GameLogs = new List<GameLog>();
        }
    }
}
