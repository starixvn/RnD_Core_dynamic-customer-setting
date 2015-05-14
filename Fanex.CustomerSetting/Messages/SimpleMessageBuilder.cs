namespace Fanex.CustomerSetting.Messages
{
    using Fanex.CustomerSetting.Models;

    public class SimpleMessageBuilder : BaseMessageBuilder, IMessageBuilder
    {
        public string Build(MessageInfo messageInfo)
        {
            var simpleMessage = Deserialize<SimpleMessage>(messageInfo.ErrMsg);
            return TranslateResource(simpleMessage.MessageId, messageInfo.SettingType);
        }
    }
}