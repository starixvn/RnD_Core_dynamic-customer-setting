namespace Fanex.CustomerSetting.Messages
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// IMessageBuilder interface.
    /// </summary>
    public interface IMessageBuilder
    {
        string Build(MessageInfo messageInfo);
    }
}