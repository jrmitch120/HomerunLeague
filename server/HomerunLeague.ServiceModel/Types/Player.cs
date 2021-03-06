﻿using System;
using System.Collections.Generic;
using HomerunLeague.ServiceModel.Utils;
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

        public int MlbTeamId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public DateTime? BirthDate { get; set; }

        public int Weight { get; set; }

        public int HeightFeet { get; set; }

        public int HeightInches { get; set; }

        public int JerseyNumber { get; set; }

        [Ignore]
        public Uri MlbProfile => MlbHelper.PlayerProfile(MlbId);

        [Ignore]
        public Uri PlayerImage => MlbHelper.PlayerImage(MlbId);

        [Ignore]
        public Uri PlayerImage2X => MlbHelper.PlayerImage(MlbId, MlbHelper.ImageSize.Large);

        [Ignore]
        public Uri TeamLogo => MlbHelper.TeamLogo(MlbTeamId);

        [Ignore]
        public Uri TeamLogo2X=> MlbHelper.TeamLogo(MlbTeamId, MlbHelper.ImageSize.Large);

        public string Bats { get; set; }

        public string PrimaryPosition { get; set; }

        public string TeamName { get; set; }

        public bool Active { get; set; }

        [Reference]
        public List<PlayerTotals> PlayerTotals { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}

