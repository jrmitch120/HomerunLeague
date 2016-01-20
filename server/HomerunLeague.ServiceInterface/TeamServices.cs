using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using HomerunLeague.ServiceInterface.Extensions;
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
            var team = Db.LoadSelect<Team>(t => t.Id == request.Id && t.Year == request.Year).SingleOrDefault();

            if (team == null)
                throw new HttpError(HttpStatusCode.NotFound, 

                    new ArgumentException("TeamId {0} does not exist in {1}. ".Fmt(request.Id, request.Year)));

            team.Players = Db.Select<Player>(q => q.Join<Player,Teamate>((player, teamate) => player.Id == teamate.PlayerId));

            return new GetTeamResponse {Team = team.ToViewModel()};
        }

        public GetTeamsResponse Get(GetTeams request) 
        {
            int page = request.Page ?? 1;

            return new GetTeamsResponse
            {
                Teams =
                    Db.LoadSelect(
                        Db.From<Team>()
                            .Where(t => t.Year == request.Year)
                            //.OrderByDescending(s => s.Totals.Hr) TODO: Crashing
                            .PageTo(page)).ToViewModel(),
                Meta =
                    new Meta(Request.AbsoluteUri)
                    {
                        Page = page,
                        TotalCount = Db.Count<Team>(t => t.Year == request.Year)
                    }
            };
        }

        public HttpResult Post(CreateTeam request)
        {
            if (!_adminSvc.Get(new GetSettings()).RegistrationOpen)
                throw new UnauthorizedAccessException("New team registration is currently closed.");

            var team = request.ConvertTo<Team>();

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
                new HttpResult(new GetTeamResponse().PopulateWith(Get(new GetTeam {Year = request.Year, Id = team.Id})))
                {
                    StatusCode = HttpStatusCode.Created,
                    Headers =
                    {
                        {HttpHeaders.Location, new GetTeam {Year = request.Year, Id = team.Id}.ToGetUrl()}
                    }
                };
        }
    }
}

