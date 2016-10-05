using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    public class TeamTotals
    {
        public int Id => TeamId;

        public int TeamId { get; set; }

        public int Ab { get; set; }

        public int Hr { get; set; }

        public int HrMovement { get; set; }
    }
}