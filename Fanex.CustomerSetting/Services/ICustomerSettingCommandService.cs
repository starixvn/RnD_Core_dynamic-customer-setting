namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// IUpdateService class.
    /// </summary>
    public interface ICustomerSettingCommandService
    {
        CommandResult Update(CommandInfo model);
    }
}