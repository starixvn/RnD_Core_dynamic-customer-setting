namespace Fanex.CustomerSetting.Client40.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Models;
    using Models.Requests;
    using RestSharp;
    using Services;

    [ExceptionHandlingAttribute, ValidateAntiForgery, LogInfoFilter]
    public class RouteController : Controller
    {
        private readonly IRouteService _routeService;

        public RouteController()
            : this(null)
        {
        }

        internal RouteController(IRouteService customerSettingService)
        {
            _routeService = customerSettingService ?? new RouteService();
        }

        [HttpPost, AdditionalRequestAuth]
        public ContentResult Redirect(RedirectRequest request)
        {
            SetActorInformation(request);

            IRestResponse updateResult = _routeService.Redirect(request);

            Response.StatusCode = (int)updateResult.StatusCode;
            Response.TrySkipIisCustomErrors = true;

            return Content(updateResult.Content);
        }

        private static void SetActorInformation(RedirectRequest request)
        {
            if (request != null)
            {
                SettingContext settingContext = CustomerSettingRegistration.SettingContext;

                request.ActorId = settingContext.ActorId;
                request.ActorLevel = settingContext.ActorLevel;
                request.ActorName = settingContext.ActorName;
                request.SubAccountId = settingContext.SubAccountId;
                request.Domain = settingContext.Domain;
                request.Language = settingContext.Language;
                request.ClientIP = settingContext.IP;
                request.System = settingContext.System;
            }
        }
    }
}