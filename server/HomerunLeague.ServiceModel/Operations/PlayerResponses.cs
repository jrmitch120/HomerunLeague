using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET RESPONSES     *
    ***********************/
    public class GetPlayerResponse : IHasResponseStatus
    {
        public Player Player { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    public class GetPlayersResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }
        public List<Player> Players { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
