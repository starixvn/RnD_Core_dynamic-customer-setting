namespace Fanex.CustomerSetting.Converters
{
    using Fanex.CustomerSetting.Entities;

    public class StatusXmlConverter : BaseXmlConverter<ProductStatusEntity>
    {
        public override string NodeName
        {
            get
            {
                return "ProductStatus";
            }
        }
    }
}