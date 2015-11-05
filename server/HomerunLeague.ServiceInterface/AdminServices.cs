using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using ServiceStack;
using ServiceStack.OrmLite;

using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;


namespace HomerunLeague.ServiceInterface
{
    public class AdminServices : Service
    {
        public AdminServices(PlayerServices playerServices)
        {
        }

        public void Post(CreateProcessingRequest request)
        {
        }
    }
}

