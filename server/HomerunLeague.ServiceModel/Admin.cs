using System;

using System.Net;
using System.Collections.Generic;

using ServiceStack;

using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.ServiceModel
{
    [Route("/admin/bioupdaterequest", "POST")]
    public class CreateProcessingRequest : ProcessingRequest, IReturn<ProcessingResponse>
    {
        
    }

    [ApiResponse(HttpStatusCode.Created, "Operation successful.")]
    public class ProcessingResponse
    {

    }
}


