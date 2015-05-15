namespace Fanex.CustomerSetting.Converters
{
    using System.Collections.Generic;
    using System.Text;
    using Fanex.CustomerSetting.Entities;

    public abstract class BaseXmlConverter<T> where T : BaseEntity
    {
        private static XmlOutputElementPropertyMapping _xmlOutputElementPropertyMapping = new XmlOutputElementPropertyMapping();

        public abstract string NodeName { get; }

        public string Generate(IEnumerable<T> entities)
        {
            var stringBuilder = new StringBuilder("<ROOT>");
            foreach (var entity in entities)
            {
                stringBuilder.Append(Generate(entity));
            }

            stringBuilder.Append("</ROOT>");
            return stringBuilder.ToString();
        }

        private string Generate(T entity)
        {
            var type = entity.GetType();
            var stringBuilder = new StringBuilder(string.Format("<{0}", NodeName));
            var objectElementProperties = _xmlOutputElementPropertyMapping.Get(type);
            foreach (var property in objectElementProperties)
            {
                var value = type.GetProperty(property.PropertyName).GetValue(entity, null);
                if (value.GetType() == typeof(bool))
                {
                    var boolValue = (bool)value;
                    value = boolValue ? 1 : 0;
                }

                stringBuilder.AppendFormat(" {0}=\"{1}\"", property.AttributeName, value);
            }

            stringBuilder.Append(" />");

            return stringBuilder.ToString();
        }
    }
}