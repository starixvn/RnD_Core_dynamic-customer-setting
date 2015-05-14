namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;
    using Fanex.CustomerSetting.Core;

    public abstract class PositionTakingEntity : BaseEntity
    {
        [OutputElement]
        public bool IsLive { get; set; }

        protected bool IsPositionTakingValue(int value)
        {
            return value >= 0 && value <= Configuration.MaxPositionTakingValue;
        }
    }
}