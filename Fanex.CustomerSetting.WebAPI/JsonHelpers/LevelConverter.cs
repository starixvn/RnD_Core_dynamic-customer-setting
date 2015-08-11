namespace Fanex.CustomerSetting.WebAPI.JsonHelpers
{
    using System;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class LevelConverter : JsonCreationConverter<Level>
    {
        protected override Level Create(Type type, JObject obj)
        {
            return Enumeration.FromValue<Level>(Convert.ToByte(obj["value"]));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken token = JToken.FromObject(value);

            token.WriteTo(writer);
        } 
    }
}