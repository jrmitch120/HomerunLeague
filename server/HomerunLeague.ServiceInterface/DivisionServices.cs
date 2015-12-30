using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    public class DivisionServices : Service
    {
        
        public GetDivisionsResponse Get(GetDivisions request) 
        {
            int page = request.Page ?? 1;

            var query = Db.From<Division>().Where(q => q.Year == request.Year);

            if (!request.IncludeInactive)
                query.And(q => q.Active);

            var divisions = Db.Select(query.PageTo(page));

            divisions.ForEach(division =>
            {
                division.Players.AddRange(
                    Db.LoadSelect<Player>(
                        q => q.Join<Player, DivisionalPlayer>((player, divPlayer) => player.Id == divPlayer.PlayerId)
                            .Where<DivisionalPlayer>(divPlayer => divPlayer.DivisionId == division.Id)));
            });
                    
            return new GetDivisionsResponse
            {
                Divisions = divisions,
                Meta = new Meta(Request.AbsoluteUri) { Page = page, TotalCount = Db.Count<Division>(query) }
            };
        }
    }
}

