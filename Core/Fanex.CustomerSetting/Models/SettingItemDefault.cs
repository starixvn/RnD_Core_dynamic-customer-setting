namespace Fanex.CustomerSetting.Models
{
    using Resource;
    using Results;

    public class SettingItemDefault : ISettingItem
    {
        private const string SportTypeBetTypeXmlFormat = "SportType=\"{0}\" BetType=\"{1}\"";

        public short SportTypeId { get; set; }

        public string SportTypeName { get; set; }

        public short BetTypeId { get; set; }

        public string BetTypeName { get; set; }

        public string TooltipMessage { get; set; }

        public bool ShowIconTooltipMessage { get; set; }

        /// <summary>
        /// For Controller create exactly concrete class when get json from client.
        /// </summary>
        public string TypeName
        {
            get
            {
                return GetType().AssemblyQualifiedName;
            }
        }

        public virtual IResult Validate()
        {
            if (SportTypeId <= 0 || BetTypeId <= 0)
            {
                return new Result(Constants.ResultValues.DataIsNotValid, Message.SportTypeOrBetTypeIsInvalid);
            }

            return Result.Success;
        }

        public virtual string ToXml()
        {
            return string.Format(SportTypeBetTypeXmlFormat, SportTypeId, BetTypeId);
        }
    }
}