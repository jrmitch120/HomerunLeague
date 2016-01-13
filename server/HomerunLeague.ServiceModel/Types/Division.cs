using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    public class Division
    {
        public Division()
        {
            Players = new List<Player>();
        }

        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public string Description { get; set; }

        public int PlayerRequirment { get; set; }

        public bool Active { get; set; }

        public int Order { get; set; }

        [Ignore]
        public List<Player> Players { get; set; }
    }
}

