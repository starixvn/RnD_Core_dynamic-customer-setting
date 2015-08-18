namespace Fanex.CustomerSetting.WebAPI.Helpers
{
    using Fanex.CustomerSetting.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class SettingConverter : JsonCreationConverter<Setting>
    {
        protected override JObject CreateJObject(JsonReader reader)
        {
            return JObject.Parse((string)reader.Value);
        }
    }
}