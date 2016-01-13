using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.GameEngine.Stats
{
    public interface IStatData 
    {
        StatPull FetchStats(Player player, int year);
    }
}