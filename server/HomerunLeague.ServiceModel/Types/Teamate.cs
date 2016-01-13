using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    [CompositeIndex("PlayerId", "TeamId", Unique=true)]
    public class Teamate
    {
        public string Id { get { return TeamId + "/" + PlayerId; } }

        public int PlayerId { get; set; }

        public int TeamId { get; set; }
    }
}

