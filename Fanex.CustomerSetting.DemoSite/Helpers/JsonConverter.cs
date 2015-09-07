namespace Fanex.CustomerSetting.DemoSite.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class JsonConverter
    {
        public static string ToJson(object data)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(data, serializerSettings);
        }
    }
}