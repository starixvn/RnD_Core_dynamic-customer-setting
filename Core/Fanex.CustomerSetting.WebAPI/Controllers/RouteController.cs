namespace Fanex.CustomerSetting.WebApi.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Models.Request;
    using Services.Impl;

    public class RouteController : ApiController
    {
        [SuppressMessage(
            "Microsoft.Globalization",
            "CA1303:Do not pass literals as localized parameters",
            Justification = "Reviewed!")]
        public HttpResponseMessage Redirect(RedirectRequest request)
        {
            if (request == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Route request cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Precondition request is failed.");
            }

            var routeService = new RouteService(request.GetSettingContext());
            var result = routeService.Redirect(request.ToRedirectCommand());

            return Request.CreateResponse(result);
        }
    }
}