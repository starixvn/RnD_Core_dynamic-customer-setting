namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    public class MasterQueryMemberResult
        : QueryResult<MasterQueryMemberCustomerSettingEntity,
            MasterQueryMemberDiscountEntity,
            MasterQueryMemberBetLimitEntity,
            MasterQueryMemberPositionTakingEntity,
            ProductStatusEntity>
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