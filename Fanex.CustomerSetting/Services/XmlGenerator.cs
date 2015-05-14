namespace Fanex.CustomerSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Attributes;
    using Fanex.CustomerSetting.Entities;

    public class XmlGenerator : IXmlGenerator
    {
        private static Dictionary<Type, IList<OutputElementProperty>> outputElementProperties = new Dictionary<Type, IList<OutputElementProperty>>();

        public string GenerateStatus(IEnumerable<ProductStatusEntity> entities)
        {
            return Generate(entities);
        }

        public string GenerateDiscount(IEnumerable<DiscountEntity> entities)
        {
            return Generate(entities);
        }

        public string GenerateBetLimit(IEnumerable<BetLimitEntity> entities)
        {
            return Generate(entities);
        }

        public string GeneratePositionTaking(IEnumerable<PositionTakingEntity> entities)
        {
            return Generate(entities);
        }

        private static IList<OutputElementProperty> GetOutputElementProperties(Type type)
        {
            if (outputElementProperties.ContainsKey(type))
            {
                return outputElementProperties[type];
            }

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

            outputElementProperties[type] = result;
            return result;
        }

        private string Generate(IEnumerable<BaseEntity> entities)
        {
            var stringBuilder = new StringBuilder("<ROOT>");
            foreach (var entity in entities)
            {
                stringBuilder.Append(Generate(entity));
            }

            stringBuilder.Append("</ROOT>");
            return stringBuilder.ToString();
        }

        private string GetNodeName(BaseEntity entity)
        {
            if (entity is ProductStatusEntity)
            {
                return "ProductStatus";
            }
            else if (entity is DiscountEntity)
            {
                return "Discount";
            }
            else if (entity is BetLimitEntity)
            {
                return "BetLimit";
            }
            else if (entity is PositionTakingEntity)
            {
                return "PT";
            }

            return string.Empty;
        }

        private string Generate(BaseEntity entity)
        {
            var type = entity.GetType();
            var nodeName = GetNodeName(entity);
            var stringBuilder = new StringBuilder(string.Format("<{0}", nodeName));
            var objectElementProperties = GetOutputElementProperties(type);
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