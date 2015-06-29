namespace Fanex.CustomerSetting.Resource.Services
{
    using Fanex.CustomerSetting.Resource.Dtos.Messages;

    public interface IMessageService
    {
        MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language);
    }
}