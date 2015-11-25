using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.GameEngine.Stats
{
    public interface IStatData 
    {
        PlayerStats FetchStats(Player player, int year);
    }
}