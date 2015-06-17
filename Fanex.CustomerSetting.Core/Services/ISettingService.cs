namespace Fanex.CustomerSetting.Core.Services
{
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public interface ISettingService
    {
        ISetting GetCustomerSetting(SettingQuery settingQuery);

        IResult UpdateCustomerSetting(SettingCommand settingCommand);

        IResult UpdateMultipleCustomerSetting(MultipleSettingCommand multipleSettingCommand);
    }
}