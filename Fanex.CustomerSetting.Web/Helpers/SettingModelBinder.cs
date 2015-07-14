namespace Fanex.CustomerSetting.Web
{
    using System;
    using System.Web.Mvc;

    public class SettingModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (bindingContext != null)
            {
                ValueProviderResult typeValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".TypeName");

                if (typeValue != null)
                {
                    Type type = Type.GetType((string)typeValue.ConvertTo(typeof(string)), true);
                    object model = Activator.CreateInstance(type);

                    bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);

                    return model;
                }
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}