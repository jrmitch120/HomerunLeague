using System;
using System.Net;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
    /***********************
    *    GET OPERATIONS    *
    ***********************/
    [Route("/seasons/{year}/gamelogs", "GET", Summary = "Get a list of game logs for a year")]
    public class GetGameLogs : PageableRequest, IReturn<GetGameLogsResponse>
    {
        [ApiMember(Name = "Year", Description = "Year of logs to get.", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Year { get; set; }

        [ApiMember(Name = "Start", Description = "Get a list of gamelogs starting from this date.", ParameterType = "query", DataType = "DateTime", IsRequired = false)]
        public DateTime? Start { get; set; }
    }

    /***********************
    *    GET OPERATIONS    *
    ***********************/
    [Route("/players/{playerId}/gamelogs", "GET", Summary = "Get a list of game logs for a player")]
    public class GetGameLogsForPlayer : PageableRequest, IReturn<GetGameLogsResponse>
    {
        [ApiMember(Name = "PlayerId", Description = "Id of player", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int PlayerId { get; set; }

        [ApiMember(Name = "Year", Description = "Year of logs to get.", ParameterType = "path", DataType = "int", IsRequired = false)]
        public int? Year { get; set; }
    }

    /***********************
    *    PUT OPERATIONS    *
    ***********************/
    [Route("/players/{playerId}/gamelogs/{gameId}", "PUT")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class PutGameLog
    {
        [ApiMember(IsRequired = true)]
        public string GameId { get; set; }
        public int PlayerId { get; set; }
        public DateTime GameDate { get; set; }
        public int G { get; set; }
        public int Ab { get; set; }
        public decimal Avg { get; set; }
        public int Bb { get; set; }
        public int Ibb { get; set; } 
        public int Cs { get; set; } 
        public int D { get; set; } 
        public int H { get; set; }
        public int Hbp { get; set; }
        public int Hr { get; set; }
        public int R { get; set; }
        public int Rbi { get; set; }
        public decimal Slg { get; set; }
        public int Sb { get; set; } 
        public int Sf { get; set; } 
        public int So { get; set; }
        public int T { get; set; } 
        public int Tb { get; set; }
        public string Team { get; set; }
        public string Opponent { get; set; }
        public string HomeAway { get; set; }
        public int TeamScore { get; set; }
        public int OppnentScore { get; set; }
        public string Result { get; set; }
    }

    [Route("/players/{playerId}/playertotals/{year}", "PUT")]
    [ApiResponse(HttpStatusCode.NoContent, "Operation successful.")]
    public class PutPlayerTotals
    {
        public int Year { get; set; }
        public int PlayerId { get; set; }
        public int G { get; set; }
        public int Ab { get; set; }
        public decimal Avg { get; set; }
        public int Bb { get; set; }
        public int Ibb { get; set; } 
        public int Cs { get; set; } 
        public int D { get; set; }
        public int H { get; set; }
        public int Hbp { get; set; }
        public int Hr { get; set; }
        public int Hr7 { get; set; }
        public int Hr14 { get; set; }
        public int Hr30 { get; set; }
        public int R { get; set; }
        public int Rbi { get; set; }
        public decimal Slg { get; set; }
        public int Sb { get; set; }
        public int Sf { get; set; }
        public int So { get; set; }
        public int T { get; set; }
        public int Tb { get; set; }
    }
}
