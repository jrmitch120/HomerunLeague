using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel
{
    [Route("/players/{playerId}/gamelogs", "PUT")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    public class PutGameLogs
    {
        [ApiMember(IsRequired = true)]
        public int PlayerId { get; set; }

        [ApiMember(IsRequired = true)]
        public List<GameLog> GameLogs { get; set; }
    }

    [Route("/players/{playerId}/playertotals", "PUT")]
    [ApiResponse(HttpStatusCode.OK, "Operation successful.")]
    public class PutPlayerTotals
    {
        [ApiMember(IsRequired = true)]
        public int PlayerId { get; set; }

        [ApiMember(IsRequired = true)]
        public PlayerTotals PlayerTotals { get; set; }
    }
}
