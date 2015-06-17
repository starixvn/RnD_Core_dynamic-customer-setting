namespace Fanex.CustomerSetting.Core.Services
{
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;

    public interface IValidationService
    {
        IResult ValidateSettingQuery(SettingQuery settingQuery);

        IResult ValidateSettingCommand(SettingCommand settingCommand);

        IResult ValidateMultipleSettingCommand(MultipleSettingCommand multipleSettingCommand);
    }
}