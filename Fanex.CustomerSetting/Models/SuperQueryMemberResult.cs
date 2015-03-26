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
        public SuperQueryMemberResult(int errCode, string errMsg, RequestContext requestContext)
            : base(errCode, errMsg, requestContext)
        {
        }

        public SuperQueryMemberResult(RequestContext requestContext, SuperQueryMemberCustomerSettingEntity customerSetting)
            : base(requestContext, customerSetting)
        {
        }
    }
}