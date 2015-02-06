namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// SuperSelectMasterResult class.
    /// </summary>
    public class MasterSelectMemberResult
        : SelectResult<MasterSelectMemberCustomerSettingEntity,
            MasterSelectMemberDiscountEntity,
            MasterSelectMemberBetLimitEntity,
            MasterSelectMemberPositionTakingEntity,
            MasterSelectMemberProductStatusEntity>
    {
        public MasterSelectMemberResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public MasterSelectMemberResult(SettingContext settingContext, MasterSelectMemberCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}