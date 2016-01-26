using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    PUT OPERATIONS    *
    ***********************/
    [Route("/players/{playerId}/gamelogs", "PUT")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class PutGameLogs
    {
        [ApiMember(IsRequired = true)]
        public int PlayerId { get; set; }

        [ApiMember(IsRequired = true)]
        public List<GameLog> GameLogs { get; set; }
    }

    [Route("/players/{playerId}/playertotals", "PUT")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class PutPlayerTotals
    {
        [ApiMember(IsRequired = true)]
        public int PlayerId { get; set; }

        [ApiMember(IsRequired = true)]
        public PlayerTotals PlayerTotals { get; set; }
    }
}
