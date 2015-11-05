using System;
using System.Collections.Generic;
using System.Linq;

using ServiceStack;
using ServiceStack.OrmLite;

using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;

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
                    Db.Select<Player>(q => q.Join<Player,DivisionalPlayer>((player, divPlayer) => player.Id == divPlayer.PlayerId)
                                            .Where<DivisionalPlayer>(divPlayer => divPlayer.DivisionId == division.Id))
                                            .ForEach(player =>
                                                {
                                                    division.Players.Add(player);
                                                });
                });
                    
            return new GetDivisionsResponse
            {
                Divisions = divisions,
                Meta = new Meta(Request.AbsoluteUri) { Page = page, TotalCount = Db.Count<Division>(query) }
            };
        }
    }
}

