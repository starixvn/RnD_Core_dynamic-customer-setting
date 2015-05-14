namespace Fanex.CustomerSetting.Web.Services
{
    using Fanex.CustomerSetting.Web.Models;

    public interface IUserService
    {
        UserContext CreateUserContext();
    }
}