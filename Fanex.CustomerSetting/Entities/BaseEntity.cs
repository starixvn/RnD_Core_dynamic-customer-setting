namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public abstract class BaseEntity
    {
        [OutputElement]
        public short SportType { get; set; }

        public string SportTypeName { get; set; }

        [OutputElement]
        public short BetType { get; set; }

        public string BetTypeName { get; set; }

        public virtual bool IsValid()
        {
            return SportType > 0 && BetType > 0;
        }
    }
}