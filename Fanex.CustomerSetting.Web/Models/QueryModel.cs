namespace Fanex.CustomerSetting.Web.Models
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// QueryModel class.
    /// </summary>
    public class QueryModel : BaseModel
    {
        /// <summary>
        /// Creates QueryInfo.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Returns QueryInfo.</returns>
        public QueryInfo CreateQueryInfo(UserContext userContext)
        {
            return new QueryInfo
            {
                CustId = userContext.CustId,
                RoleId = userContext.RoleId,
                Language = userContext.Language,
                ProductId = this.ProductId,
                FormType = this.FormType,
                TargetCustId = this.TargetCustId,
                TargetRoleId = this.TargetRoleId
            };
        }
    }
}