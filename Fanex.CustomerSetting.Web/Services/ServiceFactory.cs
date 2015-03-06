namespace Fanex.CustomerSetting.Web.Services
{
    /// <summary>
    /// ServiceFactory class.
    /// </summary>
    public static class ServiceFactory
    {
        public static IUserService CreateUserService()
        {
            return new UserService();
        }
    }
}