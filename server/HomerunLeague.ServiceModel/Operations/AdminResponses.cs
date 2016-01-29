using System.Collections.Generic;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET RESPONSES     *
    ***********************/
    public class GetSettingsResponse : IHasResponseStatus
    {
        public int BaseballYear { get; set; }

        public bool RegistrationOpen { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    public class GetLeagueEventResponse : IHasResponseStatus
    {
        public LeagueEvent LeagueEvent { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    public class GetLeagueEventsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }

        public List<LeagueEvent> LeagueEvents { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
