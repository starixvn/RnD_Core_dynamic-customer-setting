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

            var jsonContent = actionContext.Request.Content.ReadAsStringAsync().Result;

            var updateSettingRequest = JsonConvert.DeserializeObject<T>(
                jsonContent,
                new SettingConverter(),
                new SettingItemConverter(),
                new LevelConverter());

            bindingContext.Model = updateSettingRequest;
            bindingContext.ValidationNode.ValidateAllProperties = true;
            bindingContext.ValidationNode.Validate(actionContext);

            return true;
        }
    }
}