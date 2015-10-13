using System;
using ServiceStack;
using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.ServiceModel
{
    [Route("/players/{id}")]
    public class GetPlayer : IReturn<GetPlayerResponse>
    {
        public int Id { get; set; }
    }

    public class GetPlayerResponse : IHasResponseStatus 
    {
        public Player Player { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}

