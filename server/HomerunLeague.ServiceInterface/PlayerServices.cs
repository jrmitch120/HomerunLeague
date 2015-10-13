using System;
using ServiceStack;
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
		public object Any(GetPlayer request) 
		{
            return new GetPlayerResponse{ Player = new Player{ Id = request.Id, FirstName = "Jeff", LastName = "Mitchell" } };
		}
	}
}

