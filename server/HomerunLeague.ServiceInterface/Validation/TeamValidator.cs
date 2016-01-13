using System.Collections.Generic;
using System.Linq;
using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.ServiceInterface.Validation
{
    public class TeamValidator : ITeamValidator
    {
        private readonly List<Division> _divisions;

        public TeamValidator(IEnumerable<Division> divisions)
        {
            _divisions = divisions.ToList();
        }

        public bool IsValid(Team team)
        {
            foreach (var division in _divisions)
            {
                if (division.PlayerRequirment != 
                    division.Players.Intersect(team.Players, new PlayerComparer()).Count())
                    return false;
            }

            return true;
        }

        private class PlayerComparer : IEqualityComparer<Player>
        {
            public bool Equals(Player x, Player y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(Player obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
