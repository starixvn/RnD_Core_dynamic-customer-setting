namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;

    public interface IAdditionalRequestHandler
    {
        object Handle(ISettingContext settingContext, byte actionId, ActionType actionType, string jsonParameters);
    }
}