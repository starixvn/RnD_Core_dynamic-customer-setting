namespace Fanex.CustomerSetting.WebApi
{
    using System;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using Configurations;
    using Filters;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config), "HttpConfiguration cannot be null");
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
            jsonFormater.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

            config.Filters.Add(new HmacAuthorizationFilter());
            config.Filters.Add(new MaintenanceFilter());
            config.Filters.Add(new GlobalExceptionFilter());

            if (CustomerSettingAPIConfiguration.Settings.EnableRequestLogging)
            {
                config.Filters.Add(new LogInfoFilter());
            }
        }
    }
}