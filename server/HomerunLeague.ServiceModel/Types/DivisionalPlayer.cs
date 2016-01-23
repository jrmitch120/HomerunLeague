using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    [CompositeIndex("DivisionId", "PlayerId", Unique = true)]
    public class DivisionalPlayer
    {
        [AutoIncrement]
        public int Id { get; set; }

        public int DivisionId { get; set; }

        public int PlayerId { get; set; }
    }
}

