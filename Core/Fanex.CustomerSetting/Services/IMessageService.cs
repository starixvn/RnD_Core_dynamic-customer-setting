namespace Fanex.CustomerSetting.Services
{
    using Models.Messages;

    public interface IMessageService
    {
        MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language);
    }
}