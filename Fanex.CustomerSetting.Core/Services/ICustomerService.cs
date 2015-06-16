namespace Fanex.CustomerSetting.Core.Services
{
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;

    public interface ICustomerService
    {
        ISetting GetCustomerSetting(SettingQuery settingQuery);

        IResults UpdateCustomerSetting(SettingCommand settingCommand);

        IMultipleResults UpdateMultipleCustomerSetting(MultipleSettingCommand multipleSettingCommand);
    }
}