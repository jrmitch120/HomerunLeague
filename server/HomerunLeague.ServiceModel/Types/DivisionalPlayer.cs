using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
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

