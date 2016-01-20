using System.Collections.ObjectModel;
using System.Linq;
using ServiceStack.Configuration;

namespace HomerunLeague.ServiceInterface.Authentication
{
    public class ApiKeys : IKeys
    {
        public ApiKeys(IAppSettings settings)
        {
            ReadWriteApiKeys = new AppSettings().GetList("apiKeys").ToList().AsReadOnly();
        }

        public ReadOnlyCollection<string> ReadWriteApiKeys { get; }
    }
}
