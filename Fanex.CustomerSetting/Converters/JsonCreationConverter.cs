namespace Fanex.CustomerSetting.Converters
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// JSON converter class.
    /// </summary>
    /// <typeparam name="T">Type of object to convert.</typeparam>
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
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

        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object.
        /// </summary>
        /// <param name="objectType">Type of object expected.</param>
        /// <param name="jsonObject">
        /// Contents of JSON object that will be deserialized.
        /// </param>
        /// <returns>Returns object.</returns>
        protected abstract T Create(Type objectType, JObject jsonObject);
    }
}