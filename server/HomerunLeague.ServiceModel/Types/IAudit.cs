using System;

namespace HomerunLeague.ServiceModel.Types
{
    public interface IAudit
    {
        DateTime Created { get; set; }
        DateTime Modified { get; set; }
    }
}
