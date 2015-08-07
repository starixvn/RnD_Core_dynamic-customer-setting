namespace Fanex.CustomerSetting.Web
{
    using System;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Models;

    public class SettingModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (bindingContext != null)
            {
                var model = new Setting();

                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, typeof(Setting));

                return model;
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}