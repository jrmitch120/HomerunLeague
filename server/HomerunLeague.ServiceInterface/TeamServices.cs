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
    [Secured(ApplyTo.Put | ApplyTo.Delete)]
    public class TeamServices : Service
    {
        private readonly AdminServices _adminSvc;
        private readonly LeaderServices _leaderSvc;

        public TeamServices(AdminServices adminSvc, LeaderServices leaderSvc)
        {
            _adminSvc = adminSvc;
            _leaderSvc = leaderSvc;
        }

        // Get Team by Id
        public GetTeamResponse Get(GetTeam request)
        {
            var team = Db.LoadSelect<Team>(t => t.Id == request.Id).SingleOrDefault();

            if (team == null)
                throw new HttpError(HttpStatusCode.NotFound,
                    new ArgumentException("TeamId {0} does not exist. ".Fmt(request.Id)));

            var teamView = team.ToViewModel();

            teamView.TeamLeaders = _leaderSvc.Get(new GetLeadersRequest
            {
                Year = _adminSvc.Get(new GetSettings()).BaseballYear,
                TeamId = request.Id
            }).Leaders;
            
            // Let's use something a little more useful
            //Db.Select<Player>(q => q.Join<Player, Teamate>((player, teamate) => player.Id == teamate.PlayerId && 
            //                                                                    teamate.TeamId == team.Id));

            return new GetTeamResponse {Team = teamView};
        }

        // Get Teams from collection
        public GetTeamsResponse Get(GetTeams request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<Team>().Where(q => q.Year == request.Year);

            query.OrderByDescending(q => q.Year)
                 .ThenBy(q => q.Name)
                 .PageTo(page);

            return new GetTeamsResponse
            {
                Teams =
                    Db.LoadSelect(query).OrderByDescending(t => t.Totals.Hr).ToList().ToViewModel(),
                Meta =
                    new Meta(Request?.AbsoluteUri)
                    {
                        Page = page,
                        TotalCount = Db.Count(query)
                    }
            };
        }

        // Create Team
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
                new HttpResult(Get(new GetTeam {Id = team.Id}))
                {
                    StatusCode = HttpStatusCode.Created,
                    Headers =
                    {
                        {HttpHeaders.Location, new GetTeam {Id = team.Id}.ToGetUrl()}
                    }
                };
        }

        // Update Team
        public HttpResult Put(UpdateTeamTotals request)
        {
            var totals = request.TeamTotals.ConvertTo<TeamTotals>();

            totals.TeamId = request.Id;

            Db.Update(totals);

            return new HttpResult {StatusCode = HttpStatusCode.NoContent };
        }

        // Delete Team
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

        public override void Dispose()
        {
            _adminSvc.Dispose();
            base.Dispose();
        }
    }
}

