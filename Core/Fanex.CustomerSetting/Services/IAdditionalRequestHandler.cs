namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models.Enumerations;

    public interface IAdditionalRequestHandler
    {
        object Handle(byte actionId, ActionType actionType, dynamic parameters);
    }
}