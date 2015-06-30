namespace Fanex.CustomerSetting.Models
{
    using System;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Resource.Resources;

    [Serializable]
    public class DefaultSettingItem : ISettingItem
    {
        private const string SportTypeBetTypeXmlFormat = "SportType=\"{0}\" BetType=\"{1}\"";

        public short SportTypeId { get; set; }

        public string SportTypeName { get; set; }

        public short BetTypeId { get; set; }

        public string BetTypeName { get; set; }

        public virtual IResult Validate()
        {
            if (SportTypeId <= 0 || BetTypeId <= 0)
            {
                return new Result(Constants.ResultValue.DataIsNotValid, Message.SportTypeOrBetTypeIsInvalid);
            }

            return Result.Success;
        }

        public virtual string ToXml()
        {
            return string.Format(SportTypeBetTypeXmlFormat, SportTypeId, BetTypeId);
        }
    }
}