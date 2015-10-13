using System;
using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    // Basic player data
    // http://m.mlb.com/lookup/json/named.player.bam?player_id=572140&season=2015&class_id=1
    public class Player 
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

