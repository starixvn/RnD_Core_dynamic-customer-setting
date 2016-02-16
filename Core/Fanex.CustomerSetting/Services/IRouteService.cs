namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models.Enumerations;

    public interface IRouteService
    {
        object Redirect(short settingTypeId, byte actionId, ActionType actionType, dynamic parameters);
    }
}