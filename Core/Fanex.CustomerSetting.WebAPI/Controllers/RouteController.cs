namespace Fanex.CustomerSetting.WebApi.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.WebApi.Helpers;
    using Fanex.CustomerSetting.WebApi.Models.Request;

    public class RouteController : ApiController
    {
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
            var result = routeService.Redirect(request.ToRouteCommand());

            return Request.CreateResponse(result);
        }
    }
}