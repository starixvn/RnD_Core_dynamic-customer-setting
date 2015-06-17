namespace Fanex.CustomerSetting.Core.Services
{
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public interface ISettingService
    {
        ISetting GetCustomerSetting(SettingQuery settingQuery);

        IResults UpdateCustomerSetting(SettingCommand settingCommand);

        IMultipleResults UpdateMultipleCustomerSetting(MultipleSettingCommand multipleSettingCommand);
    }
}