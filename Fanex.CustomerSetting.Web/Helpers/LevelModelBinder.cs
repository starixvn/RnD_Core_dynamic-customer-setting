namespace Fanex.CustomerSetting.Web
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Models.Enumerations;

    public class LevelModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext != null)
            {
                ValueProviderResult typeValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                if (typeValue != null)
                {
                    byte value = (byte)typeValue.ConvertTo(typeof(byte));
                    object model = Level.FromValue<Level>(value);

                    bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, typeof(Level));

                    return model;
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}