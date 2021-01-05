namespace Fanex.CustomerSetting.WebApi.Helpers
{
    using System;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using Models.Request;
    using Newtonsoft.Json;

    public class UpdateSettingRequestModelBinder<T> : IModelBinder
        where T : UpdateSettingRequest
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

            if (bindingContext.ModelType != typeof(T))
            {
                return false;
            }

#pragma warning disable S4462 // Calls to "async" methods should not be blocking
            var jsonContent = actionContext.Request.Content.ReadAsStringAsync().Result;
#pragma warning restore S4462 // Calls to "async" methods should not be blocking

            var updateSettingRequest = JsonConvert.DeserializeObject<T>(
                jsonContent,
                new SettingConverter(),
                new SettingTypeConverter(),
                new SettingItemConverter(),
                new LevelConverter());

            bindingContext.Model = updateSettingRequest;
            bindingContext.ValidationNode.ValidateAllProperties = true;
            bindingContext.ValidationNode.Validate(actionContext);

            return true;
        }
    }
}