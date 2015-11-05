using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types

{
    public class ProcessingRequest
    {
        [AutoIncrement]
        public int Id { get; set; }

        public ProcessingRequestType Type { get; set; }

        public object Payload { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Completed { get; set; }
    }

    public enum ProcessingRequestType 
    {
        BioUpdate = 1
    }
}

