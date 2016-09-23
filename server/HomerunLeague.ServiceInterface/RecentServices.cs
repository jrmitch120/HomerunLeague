using System;
using HomerunLeague.ServiceInterface.Extensions;
using HomerunLeague.ServiceModel.Operations;
using HomerunLeague.ServiceModel.Types;
using HomerunLeague.ServiceModel.ViewModels;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    public class RecentServices : Service
    {
        // Get Recent HR's.  For now, just pull the last 7 days.
        public GetRecentHrResponse Get(GetRecentHrRequest request)
        {
            var since = request.Start ?? DateTime.Now.AddDays(-7);

            int page = request.Page ?? 1;

            var query = Db.From<Division>() 
                .Join<Division, DivisionalPlayer>()
                .Join<DivisionalPlayer, Player>()
                .Join<Player, GameLog>()
                .Where(d => d.Year == request.Year)
                .And(d => d.Active)
                .And<GameLog>(q => q.GameDate >= since)
                .And<GameLog>(q => q.Hr > 0)
                .OrderByDescending<GameLog>(g => g.GameDate)
                .ThenByDescending<GameLog>(g => g.Hr)
                .ThenBy<Player>(p => p.LastName);
            
            var result = Db.Select<RecentHr>(query.PageTo(page));
            
            return new GetRecentHrResponse
            {
                RecentHrs = result,
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

