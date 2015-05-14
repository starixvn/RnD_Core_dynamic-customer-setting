namespace Fanex.CustomerSetting.Messages
{
    using Fanex.CustomerSetting.Models;

    public class NormalMessageBuilder : BaseMessageBuilder, IMessageBuilder
    {
        public string Build(MessageInfo messageInfo)
        {
            return messageInfo.ErrMsg;
        }
    }
}