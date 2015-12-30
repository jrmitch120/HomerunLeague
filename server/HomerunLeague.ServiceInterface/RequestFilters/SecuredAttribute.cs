using ServiceStack;
using ServiceStack.Web;

namespace HomerunLeague.ServiceInterface.RequestFilters
{
    public class SecuredAttribute : RequestFilterAttribute
    {
        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            if (req.IsLocal)
                return;

            var appConfig = req.TryResolve<ApiKeys>();
            var apiKey = req.Headers["x-api-key"] ?? req.QueryString["api_key"];

            if (apiKey == null || !appConfig.ReadWriteApiKeys.Contains(apiKey))
            {
                throw HttpError.Unauthorized("Unauthorized.  Valid x-api-key header required.");
            }
        }
    }
}
