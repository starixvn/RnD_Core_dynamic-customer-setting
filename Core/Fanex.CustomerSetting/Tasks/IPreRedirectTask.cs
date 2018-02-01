namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Requests;

    public interface IPreRedirectTask
    {
        void Invoke(ISettingContext settingContext, RedirectCommand command);
    }
}