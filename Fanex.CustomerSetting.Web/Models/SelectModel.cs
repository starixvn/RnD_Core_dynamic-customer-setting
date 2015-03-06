namespace Fanex.CustomerSetting.Web.Models
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// SelectModel class.
    /// </summary>
    public class SelectModel : BaseModel
    {
        /// <summary>
        /// Creates SelectInfo.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Returns SelectInfo.</returns>
        public SelectInfo CreateSelectInfo(UserContext userContext)
        {
            return new SelectInfo
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