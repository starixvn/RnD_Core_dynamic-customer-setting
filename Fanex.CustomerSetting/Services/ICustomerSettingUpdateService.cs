namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// IUpdateService class.
    /// </summary>
    public interface ICustomerSettingUpdateService
    {
        UpdateResult Update(UpdateInfo model);
    }
}