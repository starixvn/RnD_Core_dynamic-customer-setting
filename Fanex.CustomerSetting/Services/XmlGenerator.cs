namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.Text;
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// XmlGenerator class.
    /// </summary>
    public class XmlGenerator : IXmlGenerator
    {
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

        private string Generate(IEnumerable<BaseEntity> entities)
        {
            var stringBuilder = new StringBuilder("<ROOT>");
            foreach (var entity in entities)
            {
                stringBuilder.Append(entity.ToXml());
            }

            stringBuilder.Append("</ROOT>");
            return stringBuilder.ToString();
        }
    }
}