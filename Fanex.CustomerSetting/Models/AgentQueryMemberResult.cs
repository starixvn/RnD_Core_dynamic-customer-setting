namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    public class AgentQueryMemberResult
        : QueryResult<AgentQueryMemberCustomerSettingEntity,
            AgentQueryMemberDiscountEntity,
            AgentQueryMemberBetLimitEntity,
            AgentQueryMemberPositionTakingEntity,
            AgentQueryMemberProductStatusEntity>
    {
        public AgentQueryMemberResult(int errCode, string errMsg, RequestContext requestContext)
            : base(errCode, errMsg, requestContext)
        {
        }

        public AgentQueryMemberResult(RequestContext requestContext, AgentQueryMemberCustomerSettingEntity customerSetting)
            : base(requestContext, customerSetting)
        {
        }
    }
}