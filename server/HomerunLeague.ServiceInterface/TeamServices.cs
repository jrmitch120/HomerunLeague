using System;
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
    public class TeamServices : Service
    {
        private readonly AdminServices _adminSvc;

        public TeamServices(AdminServices adminSvc)
        {
            _adminSvc = adminSvc;
        }

        public GetTeamResponse Get(GetTeam request)
        {
            var team = Db.LoadSelect<Team>(t => t.Id == request.Id).SingleOrDefault();

            if (team == null)
                throw new HttpError(HttpStatusCode.NotFound,

                    new ArgumentException("TeamId {0} does not exist. ".Fmt(request.Id)));

            team.Players =
                Db.Select<Player>(q => q.Join<Player, Teamate>((player, teamate) => player.Id == teamate.PlayerId));

            return new GetTeamResponse {Team = team.ToViewModel()};
        }

        public GetTeamsResponse Get(GetTeams request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<Team>();

            if (request.Year.HasValue)
                query.Where(q => q.Year == request.Year);

            query
                .OrderBy(q => new {f1 = Sql.Desc(q.Year), f2 = q.Name})
                .PageTo(page);

            return new GetTeamsResponse
            {
                Teams =
                    Db.LoadSelect(query).ToViewModel(),
                Meta =
                    new Meta(Request?.AbsoluteUri)
                    {
                        Page = page,
                        TotalCount = Db.Count(query)
                    }
            };
        }

        public HttpResult Post(CreateTeam request)
        {
            if (!_adminSvc.Get(new GetSettings()).RegistrationOpen)
                throw new UnauthorizedAccessException("New team registration is currently closed.");

            var team = request.ConvertTo<Team>();

            team.Year = _adminSvc.Get(new GetSettings()).BaseballYear;

            team.ValidationToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            using (IDbTransaction trans = Db.OpenTransaction())
            {
                Db.Save(team, references: true);

                var teamates = new List<Teamate>();

                request.PlayerIds.ForEach(id =>
                {
                    teamates.Add(new Teamate {PlayerId = id, TeamId = team.Id});
                });

                Db.InsertAll(teamates);

                trans.Commit();
            }

            return
                new HttpResult(new GetTeamResponse().PopulateWith(Get(new GetTeam {Id = team.Id})))
                {
                    StatusCode = HttpStatusCode.Created,
                    Headers =
                    {
                        {HttpHeaders.Location, new GetTeam {Id = team.Id}.ToGetUrl()}
                    }
                };
        }

        [Secured]
        public HttpResult Put(UpdateTeamTotals request)
        {
            var totals = request.TeamTotals.ConvertTo<TeamTotals>();

            totals.TeamId = request.Id;

            Db.Update(totals);

            return new HttpResult {StatusCode = HttpStatusCode.NoContent };
        }

        [Secured]
        public HttpResult Delete(DeleteTeam request)
        {
            using (IDbTransaction trans = Db.OpenTransaction())
            {
                Db.Delete<Teamate>(tm => tm.TeamId == request.Id);
                Db.DeleteById<Team>(request.Id);

                trans.Commit();
            }

            return new HttpResult {StatusCode = HttpStatusCode.NoContent};
        }
    }
}

