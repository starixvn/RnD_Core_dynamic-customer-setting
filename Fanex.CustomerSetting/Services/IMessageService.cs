namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Dtos.Messages;

    public interface IMessageService
    {
        MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language);
    }
}