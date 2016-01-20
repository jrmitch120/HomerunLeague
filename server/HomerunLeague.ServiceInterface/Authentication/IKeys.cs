using System.Collections.ObjectModel;

namespace HomerunLeague.ServiceInterface.Authentication
{
    public interface IKeys
    {
        ReadOnlyCollection<string> ReadWriteApiKeys { get; }
    }
}