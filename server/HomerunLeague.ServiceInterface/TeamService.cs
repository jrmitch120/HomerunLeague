using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using ServiceStack;
using ServiceStack.OrmLite;

using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.ServiceInterface
{
    public class TeamService : Service
    {
        public GetTeamResponse Get(GetTeam request) 
        {
            var team = Db.Single<Team>(t => t.Id == request.Id && t.Year == request.Year);

            if (team == null)
                throw new HttpError(HttpStatusCode.NotFound, 

                    new ArgumentException("TeamId {0} does not exist in {1}. ".Fmt(request.Id, request.Year)));

            team.Players = Db.Select<Player>(q => q.Join<Player,Teamate>((player, teamate) => player.Id == teamate.Id));

            return new GetTeamResponse { Team = team };
        }

        public GetTeamsResponse Get(GetTeams request) 
        {
            int page = request.Page ?? 1;

            return new GetTeamsResponse
            {
                Teams = Db.Select<Team>(q => q.PageTo(page)),
                Meta = new Meta(Request.AbsoluteUri) { Page = page, TotalCount = Db.Count<Team>() }
            };
        }

        public HttpResult Post(CreateTeam request) 
        {
            int teamId;

            using (IDbTransaction trans = Db.OpenTransaction())
            {
                teamId = (int) Db.Insert(request, selectIdentity: true);

                var teamates = new List<Teamate>();

                request.Players.ForEach(p =>
                    {
                        teamates.Add(new Teamate{PlayerId = p.Id, TeamId = teamId});
                    });

                Db.InsertAll(teamates);
              
                trans.Commit();
            }

            return new HttpResult(new GetTeamResponse {Team = Get(new GetTeam {Id = teamId}).Team})
            {
                StatusCode = HttpStatusCode.Created,
                Headers =
                    {
                        {HttpHeaders.Location, new GetTeam {Year = request.Year, Id = teamId}.ToGetUrl()}
                    }
                };
        }
    }
}

