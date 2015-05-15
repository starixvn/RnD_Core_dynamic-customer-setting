namespace Fanex.CustomerSetting.Web.Models
{
    using System;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resources;
    using Fanex.CustomerSetting.Web.Services;

    public class QueryModel : BaseModel
    {
        private readonly IUserService _userService;

        public QueryModel()
            : this(null)
        {
        }

        public QueryModel(IUserService userService)
        {
            _userService = userService ?? new UserService();
        }

        public QueryInfo CreateQueryInfo()
        {
            var userContext = _userService.CreateUserContext();
            if (userContext == null)
            {
                throw new UnauthorizedAccessException(Lang.AccessDeniedMsg);
            }

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