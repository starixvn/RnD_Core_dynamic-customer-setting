namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// CompanyQuerySuperResult class.
    /// </summary>
    public class CompanyQuerySuperResult
        : QueryResult<CompanyQuerySuperCustomerSettingEntity,
            CompanyQuerySuperDiscountEntity,
            CompanyQuerySuperBetLimitEntity,
            CompanyQuerySuperPositionTakingEntity,
            CompanyQuerySuperProductStatusEntity>
    {
        public CompanyQuerySuperResult(int errCode, string errMsg, RequestContext requestContext)
            : base(errCode, errMsg, requestContext)
        {
        }

        public CompanyQuerySuperResult(RequestContext requestContext, CompanyQuerySuperCustomerSettingEntity customerSetting)
            : base(requestContext, customerSetting)
        {
        }
    }
}