namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// SuperQueryMemberResult class.
    /// </summary>
    public class SuperQueryMemberResult
        : QueryResult<SuperQueryMemberCustomerSettingEntity,
            SuperQueryMemberDiscountEntity,
            SuperQueryMemberBetLimitEntity,
            SuperQueryMemberPositionTakingEntity,
            SuperQueryMemberProductStatusEntity>
    {
        public SuperQueryMemberResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public SuperQueryMemberResult(SettingContext settingContext, SuperQueryMemberCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}