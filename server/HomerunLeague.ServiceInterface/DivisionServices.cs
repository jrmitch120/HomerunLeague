using System;
using ServiceStack;
using ServiceStack.OrmLite;

using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.ServiceInterface
{
    public class DivisionServices : Service
    {
        private Paging _paging;

        public DivisionServices(Paging paging)
        {
            _paging = paging;
        }

        public object Get(GetDivisions request) 
        {
            int page = request.Page ?? 1;

            //var test = Db.Select<Division>(q => q.PageTo(page));
            return new GetDivisionsResponse
            {
                Divisions = Db.Select<Division>(q => q.PageTo(page)),
                Paging = new Paging(Request.AbsoluteUri) { Page = page, TotalCount = Db.Count<Division>() }
            };
        }
    }
}

