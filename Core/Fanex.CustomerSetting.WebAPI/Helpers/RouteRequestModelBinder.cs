namespace Fanex.CustomerSetting.WebApi.Helpers
{
    using System;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using Fanex.CustomerSetting.WebApi.Models.Request;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class RouteRequestModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string jsonContent = actionContext.Request.Content.ReadAsStringAsync().Result;

            RouteRequest routeRequest = JsonConvert.DeserializeObject<RouteRequest>(
                jsonContent,
                new ExpandoObjectConverter());

            bindingContext.Model = routeRequest;

            return true;
        }
    }
}