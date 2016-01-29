using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using HomerunLeague.ServiceInterface.Extensions;
using HomerunLeague.ServiceInterface.RequestFilters;
using HomerunLeague.ServiceModel.Operations;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace HomerunLeague.ServiceInterface
{
    [Secured(ApplyTo.Post | ApplyTo.Put | ApplyTo.Delete)]
    public class DivisionServices : Service
    {
        private readonly AdminServices _adminSvc;

        public DivisionServices(AdminServices adminSvc)
        {
            _adminSvc = adminSvc;
        }

        // Get Division by ID
        public GetDivisionResponse Get(GetDivision request)
        {
            var division = Db.LoadSingleById<Division>(request.Id);

            if (division == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("DivisionId {0} does not exist. ".Fmt(request.Id)));

            division.Players.AddRange(
                Db.LoadSelect<Player>(
                    q => q.Join<Player, DivisionalPlayer>((player, divPlayer) => player.Id == divPlayer.PlayerId)
                          .Where<DivisionalPlayer>(divPlayer => divPlayer.DivisionId == division.Id)));

            return new GetDivisionResponse {Division = division};
        }

        // Get Divisions from collection
        public GetDivisionsResponse Get(GetDivisions request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<Division>();
            
            if(request.Year.HasValue)
                query.And(q => q.Year == request.Year);

            if (!request.IncludeInactive)
                query.And(q => q.Active);

            query.OrderByDescending(q => q.Year)
                 .ThenBy(q => q.Order)
                 .PageTo(page);

            var divisions = Db.Select(query);

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
                    new Meta(Request?.AbsoluteUri)
                    {
                        Page = page,
                        TotalCount = Db.Count(query)
                    }
            };
        }

        
        // Create Division
        public HttpResult Post(CreateDivision request)
        {
            var baseballYear = _adminSvc.Get(new GetSettings()).BaseballYear;
            
            // Mapping.
            var division = request.ConvertTo<Division>();
            division.Year = baseballYear;

            request.PlayerIds.ForEach(playerId => division.Players.Add(new Player {Id = playerId}));
             
            var divisionPlayers = new List<DivisionalPlayer>();

            using (IDbTransaction trans = Db.OpenTransaction())
            {
                // Save the division
                Db.Save(division);

                // Create divisional player assignments.
                division.Players.ForEach(
                    player =>
                        divisionPlayers.Add(new DivisionalPlayer
                        {
                            DivisionId = division.Id,
                            PlayerId = player.Id
                        }));

                // Save divisional player assignments
                if (divisionPlayers.Any())
                    Db.SaveAll(divisionPlayers);

                trans.Commit();
            }

            return
                new HttpResult(Get(new GetDivision { Id = division.Id }))
                {
                    StatusCode = HttpStatusCode.Created,
                    Headers =
                    {
                        {HttpHeaders.Location, new GetPlayer {Id = division.Id}.ToGetUrl()}
                    }
                };
        }

        // Batch Create Divisions
        public List<HttpResult> Post(CreateDivision[] requests)
        {
            // TODO: Can't wrap in transaction because CreateDivision uses one.  This would cause nested transactions.
            var responses = requests.Map(Post);

            return responses;
        }

        // Delete Division
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

        public override void Dispose()
        {
            _adminSvc.Dispose();

            base.Dispose();
        }
    }
}

