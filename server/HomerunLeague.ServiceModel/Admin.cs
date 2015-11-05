using System.Net;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;

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


