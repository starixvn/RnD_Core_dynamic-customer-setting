namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    public class MasterQueryAgentResult
        : QueryResult<MasterQueryAgentCustomerSettingEntity,
            MasterQueryAgentDiscountEntity,
            MasterQueryAgentBetLimitEntity,
            MasterQueryAgentPositionTakingEntity,
            MasterQueryAgentProductStatusEntity>
    {
        public MasterQueryAgentResult(int errCode, string errMsg, RequestContext requestContext)
            : base(errCode, errMsg, requestContext)
        {
        }

        public MasterQueryAgentResult(RequestContext requestContext, MasterQueryAgentCustomerSettingEntity customerSetting)
            : base(requestContext, customerSetting)
        {
        }
    }
}