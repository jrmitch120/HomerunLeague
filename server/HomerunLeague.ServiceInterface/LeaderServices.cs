using HomerunLeague.ServiceInterface.Extensions;
using HomerunLeague.ServiceModel.Operations;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    public class LeaderServices : Service
	{
        // Get Player by Id
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

            return new GetLeadersResponse { Leaders = result,
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

