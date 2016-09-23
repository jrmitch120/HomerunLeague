using System;
using HomerunLeague.ServiceModel.Utils;

namespace HomerunLeague.ServiceModel.ViewModels
{
    public class Leader
    {
        public int PlayerId { get; set; }

        public int MlbId { get; set; }

        public int DivisionId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public Uri PlayerImage => MlbImages.Player(MlbId);

        public Uri PlayerImage2X => MlbImages.Player(MlbId, MlbImages.ImageSize.Large);

        /* TODO
        public Uri TeamLogo { get; set; }
        public Uri TeamLogo2X { get; set; }
        */

        public string TeamName { get; set; }

        public int Hr { get; set; }

        public int Hr7 { get; set; }

        public int Hr14 { get; set; }

        public int Hr30 { get; set; }

    }
}