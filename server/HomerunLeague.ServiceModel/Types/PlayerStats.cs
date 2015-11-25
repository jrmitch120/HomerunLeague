using System.Collections.Generic;

namespace HomerunLeague.ServiceModel.Types
{
    public class PlayerStats
    {
        public SeasonTotals SeasonTotals { get; set; }

        public List<GameLog> GameLogs { get; set; }

        public PlayerStats()
        {
            GameLogs = new List<GameLog>();
        }
    }
}
