using System;

using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    public class Division
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PlayerRequirment { get; set; }

        bool Active { get; set; }
    }
}

