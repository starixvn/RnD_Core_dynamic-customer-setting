namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// SuperSelectMasterResult class.
    /// </summary>
    public class SuperSelectMemberResult
        : SelectResult<SuperSelectMemberCustomerSettingEntity,
            SuperSelectMemberDiscountEntity,
            SuperSelectMemberBetLimitEntity,
            SuperSelectMemberPositionTakingEntity,
            SuperSelectMemberProductStatusEntity>
    {
        public SuperSelectMemberResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public SuperSelectMemberResult(SettingContext settingContext, SuperSelectMemberCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}