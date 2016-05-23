namespace Fanex.CustomerSetting.Services
{
    using Models;
    using Models.Enumerations;

    public interface IAdditionalRequestHandler
    {
        object Handle(ISettingContext settingContext, byte actionId, ActionType actionType, string jsonParameters);
    }
}