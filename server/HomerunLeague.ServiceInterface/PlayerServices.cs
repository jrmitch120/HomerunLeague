using System;
using System.Collections.Generic;
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
    public class PlayerServices : Service
	{
        // Get Player by Id
        public GetPlayerResponse Get(GetPlayer request) 
		{
            var player = Db.LoadSingleById<Player>(request.Id);

            if (player == null)
                throw new HttpError(HttpStatusCode.NotFound, new ArgumentException("PlayerId {0} does not exist. ".Fmt(request.Id)));

            // Sort the referenced stats by year.
            player.PlayerTotals.Sort((x, y) => y.Year.CompareTo(x.Year)); 

            return new GetPlayerResponse { Player = player };
		}

        // Get Players from collection
        public GetPlayersResponse Get(GetPlayers request)
        {
            int page = request.Page ?? 1;

            var query = Db.From<Player>();

            if (!request.IncludeInactive)
                query.And(q => q.Active);

            return new GetPlayersResponse
            {
                Players = Db.Select(query.PageTo(page)),
                Meta = new Meta(Request?.AbsoluteUri) { Page = page, TotalCount = Db.Count(query) }
            };
        }
        
        // Create Player
        public HttpResult Post(CreatePlayer request)
        {
            var player = request.ConvertTo<Player>();

            Db.Save(player);

            return
                new HttpResult(Get(new GetPlayer {Id = player.Id }))
                {
                    StatusCode = HttpStatusCode.Created,
                    Headers =
                    {
                        {HttpHeaders.Location, new GetPlayer {Id = player.Id}.ToGetUrl()}
                    }
                };
        }

        // Batch Create Player
        public List<HttpResult> Post(CreatePlayer[] requests)
        {
            using (var trans = Db.OpenTransaction())
            {
                var responses = requests.Map(Post);

                trans.Commit();
                return responses;
            }
        }

        // Update Player
        public HttpResult Put(PutPlayer request)
        {
            Db.Save(Get(new GetPlayer { Id = request.Id }).Player.PopulateWith(request));
            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }

        // Batch Update Player
        public List<HttpResult> Put(PutPlayer [] requests)
        {
            using (var trans = Db.OpenTransaction())
            {
                var responses = requests.Map(Put);

                trans.Commit();
                return responses;
            }
        }
    }
}

