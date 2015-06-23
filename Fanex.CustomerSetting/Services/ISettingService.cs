namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;

    public interface ISettingService
    {
        ISetting GetCustomerSetting(SettingQuery settingQuery);

        IResult UpdateCustomerSetting(SettingCommand settingCommand);

        IResult UpdateMultipleCustomerSetting(MultipleSettingCommand multipleSettingCommand);
    }
}