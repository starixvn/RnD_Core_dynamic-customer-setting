namespace Fanex.CustomerSetting.Web.Services
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Web.Models;

    public class UserService : IUserService
    {
        public UserContext CreateUserContext()
        {
            var currentSession = SessionManager.CurrentSession;
            if (currentSession == null)
            {
                return null;
            }

            return new UserContext
            {
                CustId = currentSession.CustId,
                RoleId = (RoleType)currentSession.RoleId,
                SubAccountId = currentSession.SubAccountId,
                Language = currentSession.LanguageCode
            };
        }
    }
}