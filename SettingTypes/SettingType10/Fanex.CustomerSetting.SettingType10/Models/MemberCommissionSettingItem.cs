namespace Fanex.CustomerSetting.SettingType10.Models
{
    using Fanex.CustomerSetting.SettingType10.Dtos;

    public class MemberCommissionSettingItem : SingleCommissionSettingItem
    {
        public MemberCommissionSettingItem()
        {
        }

        internal MemberCommissionSettingItem(decimal rate)
            : base(rate)
        {
        }

        internal MemberCommissionSettingItem(CommissionDto dto)
            : base(dto)
        {
            GroupId = dto.GroupId;
            GroupValue = dto.GroupValue;

            if (dto.GrandUplineRate.HasValue)
            {
                Upline.Upline = new MemberCommissionSettingItem(dto.GrandUplineRate.Value);
            }
        }

        public int GroupId { get; set; }

        public string GroupValue { get; set; }
    }
}