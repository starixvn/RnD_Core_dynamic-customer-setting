namespace Fanex.CustomerSetting.WebAPI.Helpers
{
    using System;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using Fanex.CustomerSetting.WebAPI.Models.Request;
    using Newtonsoft.Json;

    public class UpdateSettingRequestModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            if (bindingContext.ModelType != typeof(UpdateSettingRequest))
            {
                return false;
            }

            var jsonContent = actionContext.Request.Content.ReadAsStringAsync().Result;

            var updateSettingRequest = JsonConvert.DeserializeObject<UpdateSettingRequest>(
                jsonContent,
                new SettingItemConverter(),
                new LevelConverter());

            bindingContext.Model = updateSettingRequest;

            return true;
        }
    }
}