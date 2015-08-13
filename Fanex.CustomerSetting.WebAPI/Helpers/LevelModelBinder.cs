namespace Fanex.CustomerSetting.WebAPI.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.WebAPI.Models.Request;

    public class LevelModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(Level))
            {
                return false;
            }

            var value = Convert.ToByte(bindingContext.ValueProvider.GetValue("value"));

            if (value >= Level.Member.Value && value <= Level.Super.Value)
            {
                var result = Enumeration.FromValue<Level>(value);
                bindingContext.Model = result;

                return true;
            }

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Cannot convert value to Level");

            return false;
        }
    }
}