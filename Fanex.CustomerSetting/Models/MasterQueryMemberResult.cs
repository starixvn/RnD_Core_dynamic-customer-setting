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
        public MasterQueryMemberResult(int errCode, string errMsg, RequestContext requestContext)
            : base(errCode, errMsg, requestContext)
        {
        }

        public MasterQueryMemberResult(RequestContext requestContext, MasterQueryMemberCustomerSettingEntity customerSetting)
            : base(requestContext, customerSetting)
        {
        }
    }
}