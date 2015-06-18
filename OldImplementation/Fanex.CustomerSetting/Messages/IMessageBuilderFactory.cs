namespace Fanex.CustomerSetting.Messages
{
    using Fanex.CustomerSetting.Enumerations;

    public interface IMessageBuilderFactory
    {
        IMessageBuilder Create(MsgType msgType);
    }
}