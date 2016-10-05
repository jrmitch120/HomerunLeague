using System;
using HomerunLeague.ServiceModel.Utils;

namespace HomerunLeague.ServiceModel.ViewModels
{
    public class RecentHr
    {
        public int PlayerId { get; set; }

        public int MlbId { get; set; }

        public int DivisionId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public Uri PlayerImage => MlbHelper.PlayerImage(MlbId);

        public Uri PlayerImage2X => MlbHelper.PlayerImage(MlbId, MlbHelper.ImageSize.Large);

        public string TeamName { get; set; }

        public int Hr { get; set; }

        public string Opponent { get; set; }

        public string HomeAway { get; set; }

        public int TeamScore { get; set; }

        public int OppnentScore { get; set; }

        public string Result { get; set; }

        public DateTime GameDate { get; set; }
    }
}