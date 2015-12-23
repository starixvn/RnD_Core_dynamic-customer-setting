namespace Fanex.CustomerSetting.SettingType7.Models
{
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Resources;

    public class ChargedCommissionItem : SettingItemDefault
    {
        private const string ProductXmlFormat = "<ChargedCommission {0} ChargedCommission=\"{1}\" UplineChargedCommission=\"{2}\"/>";

        public ChargedCommissionItem()
        {
        }

        internal ChargedCommissionItem(ChargedCommissionDto chargedCommissionDto)
        {
            Check.Argument.IsNotNull(chargedCommissionDto, "chargedCommissionDto");

            SportTypeId = chargedCommissionDto.SportTypeId;
            BetTypeId = chargedCommissionDto.BetTypeId;
            ChargedCommission = chargedCommissionDto.ChargedCommission;
            Upline = new ChargedCommissionItem
            {
                ChargedCommission = chargedCommissionDto.UplineChargedCommission
            };
        }

        public float ChargedCommission { get; set; }

        public ChargedCommissionItem Upline { get; set; }

        public override IResult Validate()
        {
            if (Upline == null || ChargedCommission > Upline.ChargedCommission)
            {
                return new Result(
                    ErrorCode.ChargedCommissionIsGreaterThanUplineChargedCommission,
                    Message.ChargedCommissionIsGreaterThanUplineChargedCommissionError);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                ProductXmlFormat,
                base.ToXml(),
                ChargedCommission,
                Upline != null ? Upline.ChargedCommission : 0);
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as ChargedCommissionItem;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return object.Equals(SportTypeId, otherObj.SportTypeId)
                    && object.Equals(BetTypeId, otherObj.BetTypeId)
                    && object.Equals(ChargedCommission, otherObj.ChargedCommission);
            }
        }

        public override int GetHashCode()
        {
            return SportTypeId.GetHashCode() ^ BetTypeId.GetHashCode() ^ ChargedCommission.GetHashCode();
        }
    }
}