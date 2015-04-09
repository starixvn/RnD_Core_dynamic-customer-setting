namespace Fanex.CustomerSetting.Messages
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// NormalMessageBuilder class.
    /// </summary>
    public class NormalMessageBuilder : BaseMessageBuilder, IMessageBuilder
    {
        public string Build(MessageInfo messageInfo)
        {
            return messageInfo.ErrMsg;
        }
    }
}