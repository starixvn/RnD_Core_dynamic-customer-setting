namespace Fanex.CustomerSetting.WebApi.Helpers
{
    using System;
    using Fanex.CustomerSetting.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class SettingConverter : JsonCreationConverter<Setting>
    {
        protected override JObject CreateJObject(JsonReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader", "Reader cannot be null");
            }

            return JObject.Parse((string)reader.Value);
        }
    }
}