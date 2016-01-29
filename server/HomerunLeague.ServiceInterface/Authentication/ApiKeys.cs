using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HomerunLeague.ServiceInterface.Authentication
{
    public class ApiKeys : IKeys
    {
        public ApiKeys(IEnumerable<string> keys)
        {
            ReadWriteApiKeys = keys.ToList().AsReadOnly();
        }

        public ReadOnlyCollection<string> ReadWriteApiKeys { get; }
    }
}
