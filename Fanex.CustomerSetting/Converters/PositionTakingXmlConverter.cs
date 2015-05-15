namespace Fanex.CustomerSetting.Converters
{
    using Fanex.CustomerSetting.Entities;

    public class PositionTakingXmlConverter : BaseXmlConverter<PositionTakingEntity>
    {
        public override string NodeName
        {
            get
            {
                return "PT";
            }
        }
    }
}