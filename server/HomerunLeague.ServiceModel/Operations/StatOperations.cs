using System;
using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

namespace HomerunLeague.ServiceModel.Operations
{
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
    }
}
