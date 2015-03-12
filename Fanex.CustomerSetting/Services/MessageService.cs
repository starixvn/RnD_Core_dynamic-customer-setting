namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Messages;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resources;

    /// <summary>
    /// MessageService class.
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly IResourceService _resourceService;

        private Dictionary<SettingType, string> _successMsgs = new Dictionary<SettingType, string>
        {
            { SettingType.Status, Lang.UpdateProductStatusSuccessfully },
            { SettingType.Discount, Lang.UpdateCommissionSuccessfully },
            { SettingType.BetLimit, Lang.UpdateBetSettingSuccessfully },
            { SettingType.PositionTaking, Lang.UpdatePositionTakingSuccessfully }
        };

        private Dictionary<SettingType, string> _prefixs = new Dictionary<SettingType, string>
        {
            { SettingType.Status, "Status" },
            { SettingType.Discount, "Discount" },
            { SettingType.BetLimit, "BetLimit" },
            { SettingType.PositionTaking, "PositionTaking" }
        };

        public MessageService(string languageCode)
            : this(null, languageCode)
        {
        }

        public MessageService(IResourceService resourceService, string languageCode)
        {
            _resourceService = resourceService ?? new ResourceService(languageCode);
        }

        /// <summary>
        /// Gets the translated message.
        /// </summary>
        /// <param name="messageInfo">The message information.</param>
        /// <returns>
        /// Returns translated message.
        /// </returns>
        public string GetTranslatedMessage(MessageInfo messageInfo)
        {
            if (messageInfo.ErrCode == Result.SuccessValue)
            {
                return _successMsgs[messageInfo.SettingType];
            }

            switch ((MsgType)messageInfo.MsgType)
            {
                case MsgType.Simple:
                    return TranslateSimpleMessage(messageInfo);

                case MsgType.Status:
                case MsgType.Discount:
                case MsgType.BetLimit:
                case MsgType.PositionTaking:
                    return TranslateTemplateMessage<NormalMessage, SimpleRow>(messageInfo);

                case MsgType.PositionTakingDownline:
                    return TranslateTemplateMessage<PositionTakingDownlineMessage, PositionTakingDownlineRow>(messageInfo);

                case MsgType.PositionTakingMin:
                    return TranslateTemplateMessage<PositionTakingMinMessage, PositionTakingMinRow>(messageInfo);

                case MsgType.PositionTakingMax:
                    return TranslateTemplateMessage<PositionTakingMaxMessage, PositionTakingMaxRow>(messageInfo);

                case MsgType.PositionTakingMinMax:
                    return TranslateTemplateMessage<PositionTakingMinMaxMessage, PositionTakingMinMaxRow>(messageInfo);

                case MsgType.Other:
                    return messageInfo.ErrMsg;
            }

            return messageInfo.ErrMsg;
        }

        private string TranslateSimpleMessage(MessageInfo messageInfo)
        {
            var simpleMessage = Deserialize<SimpleMessage>(messageInfo.ErrMsg);
            return TranslateResource(simpleMessage.MessageId, messageInfo.SettingType);
        }

        /// <summary>
        /// Translates the template message.
        /// </summary>
        /// <typeparam name="M">Type of template message.</typeparam>
        /// <typeparam name="R">Type of message row.</typeparam>
        /// <param name="messageInfo">The message information.</param>
        /// <returns>
        /// Returns translate message.
        /// </returns>
        private string TranslateTemplateMessage<M, R>(MessageInfo messageInfo)
            where M : TemplateMessage<R>
            where R : SimpleRow
        {
            var templateMessage = Deserialize<M>(messageInfo.ErrMsg);
            var message = TranslateResource(templateMessage.MessageId, messageInfo.SettingType);
            if (messageInfo.IsMultiple)
            {
                message = string.Format("{0}: {1}", messageInfo.UserName, message);
            }

            var stringBuilder = new StringBuilder(message);
            if (messageInfo.SettingLevel != SettingLevel.Product)
            {
                if (typeof(R) == typeof(PositionTakingDownlineRow))
                {
                }
                else
                {
                    foreach (var row in templateMessage.Rows)
                    {
                        stringBuilder.AppendFormat("<br />{0}", row.ToString(_resourceService, messageInfo));
                    }
                }
            }

            return stringBuilder.ToString();
        }

        private string TranslateResource(int messageId, SettingType settingType)
        {
            var prefix = _prefixs[settingType];
            var key = string.Format("{0}_{1}", prefix, messageId);
            var message = Error.ResourceManager.GetString(key);
            return !string.IsNullOrEmpty(message)
                ? message
                : string.Format("Undefined error message {0}", messageId);
        }

        private T Deserialize<T>(string text) where T : SimpleMessage
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(text))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}