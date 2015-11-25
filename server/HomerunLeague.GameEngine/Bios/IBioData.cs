using System.Collections.Generic;
using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.GameEngine.Bios
{
    public interface IBioData 
    {
        void UpdatePlayerBios(IEnumerable<Player> players);
    }
}