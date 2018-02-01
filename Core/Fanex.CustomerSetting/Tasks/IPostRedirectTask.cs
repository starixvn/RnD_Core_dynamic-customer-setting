namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Requests;

    public interface IPostRedirectTask
    {
        void Invoke(ISettingContext settingContext, RedirectCommand command, object result);
    }
}