namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// IMessageService class.
    /// </summary>
    public interface IMessageService
    {
        string GetTranslatedMessage(MessageInfo messageInfo);
    }
}