using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using HomerunLeague.ServiceInterface.Extensions;
using HomerunLeague.ServiceInterface.RequestFilters;
using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    public class DivisionServices : Service
    {
        private readonly AdminServices _adminSvc;

        public DivisionServices(AdminServices adminSvc)
        {
            _adminSvc = adminSvc;
        }

        public GetDivisionsResponse Get(GetDivisions request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<Division>();
            
            if(request.Year.HasValue)
                query.And(q => q.Year == request.Year);

            if (!request.IncludeInactive)
                query.And(q => q.Active);

            query.OrderBy(q => new {f1 = Sql.Desc(q.Year), f2 = q.Order})
                .PageTo(page);

            var divisions = Db.Select(query.OrderBy(q => q.Order).PageTo(page));

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
                Meta =
                    new Meta(Request != null ? Request.AbsoluteUri : string.Empty)
                    {
                        Page = page,
                        TotalCount = Db.Count(query)
                    }
            };
        }

        [Secured]
        public HttpResult Post(CreateDivisions request)
        {
            var baseballYear = _adminSvc.Get(new GetSettings()).BaseballYear;

            // Mapping.  
            var divisions = request.Divisions.ConvertAll(incoming =>
            {
                var division = incoming.ConvertTo<Division>();

                division.Year = baseballYear;

                incoming.PlayerIds.ForEach(divPlayerId => division.Players.Add(new Player {Id = divPlayerId}));
                return division;
            });

            var divisionPlayers = new List<DivisionalPlayer>();

            using (IDbTransaction trans = Db.OpenTransaction())
            {
                // Save the divisions
                Db.SaveAll(divisions);

                // Loop the divisions and create divisional player assignments for each divisions
                divisions.ForEach(
                    division =>
                        division.Players.ForEach(
                            player =>
                                divisionPlayers.Add(new DivisionalPlayer
                                {
                                    DivisionId = division.Id,
                                    PlayerId = player.Id
                                })));

                // Save divisional player assignments
                if (divisionPlayers.Any())
                    Db.SaveAll(divisionPlayers);

                trans.Commit();
            }

            return new HttpResult {StatusCode = HttpStatusCode.Created};
        }

        [Secured]
        public HttpResult Delete(DeleteDivision request)
        {
            using (IDbTransaction trans = Db.OpenTransaction())
            {
                Db.Delete<DivisionalPlayer>(dp => dp.DivisionId == request.Id);
                Db.DeleteById<Division>(request.Id);

                trans.Commit();
            }

            return new HttpResult {StatusCode = HttpStatusCode.NoContent};
        }
    }
}

