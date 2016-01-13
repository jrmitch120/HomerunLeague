using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.ServiceInterface.Validation
{
    public interface ITeamValidator
    {
        bool IsValid(Team team);
    }
}