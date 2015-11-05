using System;
using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    [CompositeIndex("PlayerId", "TeamId", Unique=true)]
    public class Teamate
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Player))]
        public int PlayerId { get; set; }

        [References(typeof(Team))]
        public int TeamId { get; set; }
    }
}

