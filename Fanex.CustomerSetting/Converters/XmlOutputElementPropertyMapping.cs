namespace Fanex.CustomerSetting.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Attributes;

    public class XmlOutputElementPropertyMapping
    {
        private Dictionary<Type, IList<OutputElementProperty>> outputElementProperties = new Dictionary<Type, IList<OutputElementProperty>>();

        public IList<OutputElementProperty> Get(Type type)
        {
            if (!Has(type))
            {
                Add(type);
            }

            return outputElementProperties[type];
        }

        public bool Has(Type type)
        {
            return outputElementProperties.ContainsKey(type);
        }

        public void Add(Type type)
        {
            var result = Create(type);
            outputElementProperties[type] = result;
        }

        private IList<OutputElementProperty> Create(Type type)
        {
            var result = new List<OutputElementProperty>();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                var attribute = (OutputElementAttribute)property.GetCustomAttributes(typeof(OutputElementAttribute), true).FirstOrDefault();
                if (attribute != null)
                {
                    var propertyName = property.Name;
                    var attributeName = string.IsNullOrEmpty(attribute.Name) ? property.Name : attribute.Name;
                    result.Add(new OutputElementProperty
                    {
                        PropertyName = propertyName,
                        AttributeName = attributeName
                    });
                }
            }

            return result;
        }
    }
}