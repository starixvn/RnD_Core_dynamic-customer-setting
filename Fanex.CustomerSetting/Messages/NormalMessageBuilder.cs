namespace Fanex.CustomerSetting.Messages
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingTypes;

    public class NormalMessageBuilder : BaseMessageBuilder, IMessageBuilder
    {
        public NormalMessageBuilder()
            : base()
        {
        }

        public NormalMessageBuilder(ISettingHandlerFactory settingHandlerFactory)
            : base(settingHandlerFactory)
        {
        }

        public string Build(MessageInfo messageInfo)
        {
            return messageInfo.ErrMsg;
        }
    }
}