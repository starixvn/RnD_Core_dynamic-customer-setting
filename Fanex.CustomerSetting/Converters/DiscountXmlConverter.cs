namespace Fanex.CustomerSetting.Converters
{
    using Fanex.CustomerSetting.Entities;

    public class DiscountXmlConverter : BaseXmlConverter<DiscountEntity>
    {
        public override string NodeName
        {
            get
            {
                return "Discount";
            }
        }
    }
}