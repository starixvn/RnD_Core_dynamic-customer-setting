namespace Fanex.CustomerSetting.WebAPI.Helpers
{
    using System;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected virtual T Create(Type type, JObject obj)
        {
            Type targetType = GetTargetType(type, obj);

            return (T)Activator.CreateInstance(targetType);
        }

        protected virtual Type GetTargetType(Type type, JObject obj)
        {
            return type;
        }

        public override bool CanConvert(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            ValidateInputParams(reader, type, serializer);

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            JObject obj = JObject.Load(reader);
            T target = Create(type, obj);
            JsonReader jsonReader = obj.CreateReader();
            jsonReader.FloatParseHandling = reader.FloatParseHandling;
            serializer.Populate(jsonReader, target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        private void ValidateInputParams(JsonReader reader, Type type, JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (type == null)
            {
                throw new ArgumentNullException("objectType");
            }

            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }
        }
    }
}