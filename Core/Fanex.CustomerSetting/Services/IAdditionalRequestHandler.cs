namespace Fanex.CustomerSetting.Services
{
    using Models;
    using Models.Requests;

    public interface IAdditionalRequestHandler
    {
        object Handle(ISettingContext settingContext, RedirectCommand command);
    }
}