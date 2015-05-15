namespace Fanex.CustomerSetting.Converters
{
    using Fanex.CustomerSetting.Entities;

    public class BetLimitXmlConverter : BaseXmlConverter<BetLimitEntity>
    {
        public override string NodeName
        {
            get
            {
                return "BetLimit";
            }
        }
    }
}