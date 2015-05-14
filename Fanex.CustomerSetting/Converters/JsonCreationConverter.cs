namespace Fanex.CustomerSetting.Converters
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            var jsonObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jsonObject);

            // Populate the object properties
            serializer.Populate(jsonObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected abstract T Create(Type objectType, JObject jsonObject);
    }
}