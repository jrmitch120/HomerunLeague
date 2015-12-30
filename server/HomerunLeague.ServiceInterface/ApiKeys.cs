using System.Collections.Generic;
using System.Linq;

namespace HomerunLeague.ServiceInterface
{
    public class ApiKeys
    {
        public ApiKeys()
        {
            ReadWriteApiKeys = new List<string>();
        }

        public ApiKeys(string[] keys)
        {
            ReadWriteApiKeys = keys.ToList();
        }

        public List<string> ReadWriteApiKeys { get; private set; }
    }
}
