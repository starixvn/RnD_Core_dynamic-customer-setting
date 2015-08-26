namespace Fanex.CustomerSetting.WebAPI
{
    using System;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.ModelBinding.Binders;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.WebAPI.Filters;
    using Fanex.CustomerSetting.WebAPI.Helpers;
    using Fanex.CustomerSetting.WebAPI.Models.Request;
    using Newtonsoft.Json.Serialization;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config", "HttpConfiguration cannot be null");
            }

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApiByAction",
                routeTemplate: "api/{controller}/{action}");

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var formaters = GlobalConfiguration.Configuration.Formatters;
            formaters.Clear();
            formaters.Add(new JsonMediaTypeFormatter());

            var jsonFormater = formaters.JsonFormatter;
            jsonFormater.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new GlobalExceptionFilterAttribute());
        }
    }
}
