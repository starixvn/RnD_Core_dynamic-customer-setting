namespace Fanex.CustomerSetting.Web.Services
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Web.Models;

    /// <summary>
    /// UserService class.
    /// </summary>
    public class UserService : IUserService
    {
        private ICurrentSession _currentSession;

        public UserService()
            : this(null)
        {
        }

        public UserService(ICurrentSession currentSession)
        {
            _currentSession = currentSession ?? new CurrentSession();
        }

        public UserContext CreateUserContext()
        {
            return new UserContext
            {
                CustId = _currentSession.CustId,
                RoleId = (RoleType)_currentSession.RoleId,
                SubAccountId = _currentSession.SubAccountId,
                Language = _currentSession.LanguageCode
            };
        }
    }
}