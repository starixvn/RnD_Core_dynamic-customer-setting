namespace Fanex.CustomerSetting.SettingTypes
{
    public interface ISettingHandler
    {
        string SuccessMsg { get; }

        string ErrorMsgPrefix { get; }
    }
}