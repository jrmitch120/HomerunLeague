using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.GameEngine.Valiadtion
{
    public interface ITeamValidator
    {
        bool IsValid(Team team);
    }
}