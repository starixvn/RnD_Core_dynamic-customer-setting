namespace Fanex.CustomerSetting.Web.Services
{
    public static class ServiceFactory
    {
        public static IUserService CreateUserService()
        {
            return new UserService();
        }
    }
}