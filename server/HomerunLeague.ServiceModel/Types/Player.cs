using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    // Basic player data
    // http://m.mlb.com/lookup/json/named.player.bam?player_id=572140
    public class Player : IAudit
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Index(Unique=true)]
        public int MlbId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public int Weight { get; set; }

        public int HeightFeet { get; set; }

        public int HeightInches { get; set; }

        public int JerseyNumber { get; set; }

        public string Bats { get; set; }

        public string PrimaryPosition { get; set; }

        public string TeamName { get; set; }

        public bool Active { get; set; }

        [Reference]
        public List<SeasonTotals> SeasonTotals { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}

