namespace Fanex.CustomerSetting.WebAPI
{
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using Newtonsoft.Json.Serialization;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var formaters = GlobalConfiguration.Configuration.Formatters;
            formaters.Clear();
            formaters.Add(new JsonMediaTypeFormatter());
            var jsonFormater = formaters.JsonFormatter;
            jsonFormater.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
