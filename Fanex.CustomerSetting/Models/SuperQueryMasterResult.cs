namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// SuperQueryMasterResult class.
    /// </summary>
    public class SuperQueryMasterResult
        : QueryResult<SuperQueryMasterCustomerSettingEntity,
            SuperQueryMasterDiscountEntity,
            SuperQueryMasterBetLimitEntity,
            SuperQueryMasterPositionTakingEntity,
            SuperQueryMasterProductStatusEntity>
    {
        public SuperQueryMasterResult(int errCode, string errMsg, RequestContext requestContext)
            : base(errCode, errMsg, requestContext)
        {
        }

        public SuperQueryMasterResult(RequestContext requestContext, SuperQueryMasterCustomerSettingEntity customerSetting)
            : base(requestContext, customerSetting)
        {
        }
    }
}