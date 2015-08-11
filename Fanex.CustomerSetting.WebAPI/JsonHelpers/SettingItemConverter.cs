namespace Fanex.CustomerSetting.WebAPI.JsonHelpers
{
    using System;
    using Fanex.CustomerSetting.Models;
    using Newtonsoft.Json.Linq;

    public class SettingItemConverter : JsonCreationConverter<ISettingItem>
    {
        protected override Type GetTargetType(Type type, JObject obj)
        {
            return Type.GetType(obj.GetValue("typeName").ToString());
        }
    }
}