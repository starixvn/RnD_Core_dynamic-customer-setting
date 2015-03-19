namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// MasterQueryMemberResult class.
    /// </summary>
    public class MasterQueryMemberResult
        : QueryResult<MasterQueryMemberCustomerSettingEntity,
            MasterQueryMemberDiscountEntity,
            MasterQueryMemberBetLimitEntity,
            MasterQueryMemberPositionTakingEntity,
            MasterQueryMemberProductStatusEntity>
    {
        public MasterQueryMemberResult(int errCode, string errMsg, SettingContext settingContext)
            : base(errCode, errMsg, settingContext)
        {
        }

        public MasterQueryMemberResult(SettingContext settingContext, MasterQueryMemberCustomerSettingEntity customerSetting)
            : base(settingContext, customerSetting)
        {
        }
    }
}