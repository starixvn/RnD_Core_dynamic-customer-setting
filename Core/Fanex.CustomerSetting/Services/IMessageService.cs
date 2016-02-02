namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models.Messages;

    public interface IMessageService
    {
        MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language);
    }
}