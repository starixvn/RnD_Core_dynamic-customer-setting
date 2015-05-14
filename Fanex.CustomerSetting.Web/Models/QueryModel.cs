namespace Fanex.CustomerSetting.Web.Models
{
    using Fanex.CustomerSetting.Models;

    public class QueryModel : BaseModel
    {
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