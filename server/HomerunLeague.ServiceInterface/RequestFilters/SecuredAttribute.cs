using HomerunLeague.ServiceInterface.Authentication;
using ServiceStack;
using ServiceStack.Web;

namespace HomerunLeague.ServiceInterface.RequestFilters
{
    public class SecuredAttribute : RequestFilterAttribute
    {
        public SecuredAttribute() { }

        public SecuredAttribute(ApplyTo applyTo) : base(applyTo) { }

        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            if (req.IsLocal)
                return;

            var apiKeys = req.TryResolve<IKeys>();
            var apiKey = req.Headers["x-api-key"] ?? req.QueryString["api_key"];

            if (apiKey == null || !apiKeys.ReadWriteApiKeys.Contains(apiKey))
            {
                throw HttpError.Unauthorized("Unauthorized.  Valid x-api-key header required.");
            }
        }
    }
}
