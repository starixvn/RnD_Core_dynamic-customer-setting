namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;

    public interface IValidationService
    {
        IResult ValidateSettingQuery(SettingQuery settingQuery);

        IResult ValidateSettingCommand(SettingCommand settingCommand);

        IResult ValidateMultipleSettingCommand(MultipleSettingCommand multipleSettingCommand);
    }
}