using System;
using ServiceStack.DataAnnotations;

using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.ServiceModel
{
    [CompositeIndex("PlayerId", "DivisionId", Unique = true)]
    public class DivisionalPlayer
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Player))]
        public int PlayerId { get; set; }

        [References(typeof(Division))]
        public int DivisionId { get; set; }
    }
}

