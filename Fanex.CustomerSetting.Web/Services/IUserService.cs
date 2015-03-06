namespace Fanex.CustomerSetting.Web.Services
{
    using Fanex.CustomerSetting.Web.Models;

    /// <summary>
    /// IUserService class.
    /// </summary>
    public interface IUserService
    {
        UserContext CreateUserContext();
    }
}