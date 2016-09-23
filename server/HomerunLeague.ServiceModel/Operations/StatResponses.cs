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
    
    public class GetGameLogsResponse : IHasResponseStatus, IMeta
    {
        public Meta Meta { get; set; }
        public List<GameLog> GameLogs { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
