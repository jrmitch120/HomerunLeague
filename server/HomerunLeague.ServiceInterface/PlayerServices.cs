using System;
using System.Net;
using ServiceStack;
using ServiceStack.OrmLite;

using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;
/*

/{year}/league
/{year}/league/divisions
/{year}/league/divisions/{id}/players

/players/{id}

*/

namespace HomerunLeague.ServiceInterface
{
    public class PlayerServices : Service
	{
        public GetPlayerResponse Get(GetPlayer request) 
		{
            var player = Db.LoadSingleById<Player>(request.Id);

            if (player == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("PlayerId {0} does not exist. ".Fmt(request.Id)));

            return new GetPlayerResponse { Player = player };
		}

        public GetPlayersResponse Get(GetPlayers request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<Player>();

            if (!request.IncludeInactive)
                query.And(q => q.Active);

            return new GetPlayersResponse
            {
                Players = Db.LoadSelect(query.PageTo(page)),
                Meta = new Meta(Request != null ? Request.AbsoluteUri : string.Empty) { Page = page, TotalCount = Db.Count(query) }
            };
        }

        public HttpResult Put(PutPlayers request)
        {
            Db.SaveAll(request.Players);
            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }
	}
}

