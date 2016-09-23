using System;
using HomerunLeague.ServiceInterface.Extensions;
using HomerunLeague.ServiceModel.Operations;
using HomerunLeague.ServiceModel.Types;
using HomerunLeague.ServiceModel.ViewModels;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    public class LeaderServices : Service
    {
        // Get Leaders.  This version is unfilterested and pulls it's data from a summary table
        public GetLeadersResponse Get(GetLeadersRequest request)
        {
            int page = request.Page ?? 1;

            var query =
                Db.From<Division>()
                    .Join<Division, DivisionalPlayer>()
                    .Join<DivisionalPlayer, Player>();

            if (request.TeamId.HasValue)
                query.Join<Player, Teamate>((p, t) => p.Id == t.PlayerId && t.TeamId == request.TeamId.Value);

            query.LeftJoin<Player, PlayerTotals>((c, t) => c.Id == t.PlayerId && t.Year == request.Year)
                .Where<Division>(d => d.Year == request.Year)
                .OrderByDescending<PlayerTotals>(t => t.Hr)
                .ThenBy<Player>(p => p.LastName);
            
            var result = Db.Select<Leader>(query.PageTo(page));
            
            return new GetLeadersResponse
            {
                Leaders = result,
                Meta =
                    new Meta(Request?.AbsoluteUri)
                    {
                        Page = page,
                        TotalCount = Db.Count(query)
                    }
            };
        }

        // Get Leaders.  This version is filtered by date range, so it's a little more intensive and requires us
        // to run calculations directly on the GameLogs.
        public GetLeadersResponse Get(GetLeadersByDateRequest request)
        {
            // TODO:  REMINDER.  GameLog date is NOT in Utc.

            // TODO: Figure out how to "ormlite" this.
            //var result = Db.Select<Leader>(
            //    "SELECT \"Player\".\"DisplayName\", \"Player\".\"TeamName\", coalesce(Sum(\"GameLog\".Hr),0) as HR\r\n" +
            //    "                FROM \"Division\"\r\n" +
            //    "                  INNER JOIN \"DivisionalPlayer\" ON (\"Division\".\"Id\" = \"DivisionalPlayer\".\"DivisionId\")\r\n" +
            //    "                  INNER JOIN \"Player\" ON (\"Player\".\"Id\" = \"DivisionalPlayer\".\"PlayerId\")\r\n" +
            //    "                  LEFT JOIN \"GameLog\"\r\n" +
            //    "                    ON ((\"Player\".\"Id\" = \"GameLog\".\"PlayerId\")\r\n" +
            //    "                         AND \"GameLog\".\"GameDate\" BETWEEN DATETIME(\'2016-05-01 00:00:00\') AND DATETIME(\'2016-05-01 23:59:59\')\r\n                    )\r\n                WHERE (\"Division\".\"Year\" = 2016)\r\n" +
            //    "                  GROUP BY  \"Player\".Id\r\n" +
            //    "                ORDER BY \"HR\" DESC, \"Player\".\"LastName\"");

            int page = request.Page ?? 1;

            var query =
                Db.From<Division>()
                    .Join<Division, DivisionalPlayer>()
                    .Join<DivisionalPlayer, Player>();

            query.LeftJoin<Player, GameLog>((c, gl) => c.Id == gl.PlayerId)
                .Where<Division>(d => d.Year == request.Year)
                .And<GameLog>(gl => gl.GameDate > DateTime.Now.AddDays(-5))
                .GroupBy("PlayerId")
                .OrderByDescending<PlayerTotals>(t => t.Hr)
                .ThenBy<Player>(p => p.LastName);

                /*
                 public int PlayerId { get; set; }

        public int DivisionId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string TeamName { get; set; }

        public int Hr { get; set; } */
            var sql = query.ToSelectStatement();
        var result = Db.Select<Leader>(query.PageTo(page));

            

            return new GetLeadersResponse
            {
                Leaders = result,
                Meta =
                    new Meta(Request?.AbsoluteUri)
                    {
                        Page = page,
                        TotalCount = Db.Count(query)
                    }
            };
        }
    }
}

