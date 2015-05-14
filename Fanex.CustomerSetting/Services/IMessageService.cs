namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    public interface IMessageService
    {
        string GetTranslatedMessage(MessageInfo messageInfo);
    }
}