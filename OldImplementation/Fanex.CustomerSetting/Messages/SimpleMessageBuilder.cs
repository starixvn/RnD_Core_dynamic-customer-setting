namespace Fanex.CustomerSetting.Messages
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingTypes;

    public class SimpleMessageBuilder : BaseMessageBuilder, IMessageBuilder
    {
        public SimpleMessageBuilder()
            : base()
        {
        }

        public SimpleMessageBuilder(ISettingHandlerFactory settingHandlerFactory)
            : base(settingHandlerFactory)
        {
        }

        public string Build(MessageInfo messageInfo)
        {
            var simpleMessage = Deserialize<SimpleMessage>(messageInfo.ErrMsg);
            return TranslateResource(simpleMessage.MessageId, messageInfo.SettingType);
        }
    }
}