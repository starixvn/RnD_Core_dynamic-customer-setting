namespace Fanex.CustomerSetting.Messages
{
    using Fanex.CustomerSetting.Models;

    public interface IMessageBuilder
    {
        string Build(MessageInfo messageInfo);
    }
}