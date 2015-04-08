namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
                    return TranslateTemplateMessage<NormalMessage, SimpleRow>(messageInfo);

                case MsgType.PositionTaking:
                    return TranslateTemplateMessage<PositionTakingMessage, PositionTakingRow>(messageInfo);

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
                    var downlineRows = templateMessage.Rows.Cast<PositionTakingDownlineRow>();
                    stringBuilder.Append(TranslatePositionTakingDownlineRows(templateMessage.MessageId, downlineRows, messageInfo));
                }
                else
                {
                    foreach (var row in templateMessage.Rows)
                    {
                        stringBuilder.AppendFormat("<div class='row'>{0}</div>", row.ToString(_resourceService, messageInfo));
                    }
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Translates the position taking down-line rows.
        /// </summary>
        /// <param name="messageId">The message ID.</param>
        /// <param name="rows">The position taking down-line rows.</param>
        /// <param name="messageInfo">The message information.</param>
        /// <returns>Returns translated message.</returns>
        private string TranslatePositionTakingDownlineRows(int messageId, IEnumerable<PositionTakingDownlineRow> rows, MessageInfo messageInfo)
        {
            var groups = rows.GroupBy(row => new
            {
                row.SportType,
                row.BetType,
                row.IsLive
            }).Select(group => new PositionTakingDownlineGroup
            {
                SportType = group.Key.SportType,
                BetType = group.Key.BetType,
                IsLive = group.Key.IsLive,
                Rows = group.ToList()
            });

            var stringBuilder = new StringBuilder();
            foreach (var group in groups)
            {
                var roleTypeName = GetRoleTypeName(messageId);
                var userNames = string.Join(", ", group.Rows.Select(row => row.UserName));
                stringBuilder.AppendFormat("<br />{0} - {1}: {2}", group.ToString(_resourceService, messageInfo), roleTypeName, userNames);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the name of the role type.
        /// </summary>
        /// <param name="messageId">The message ID.</param>
        /// <returns>Returns name of role type.</returns>
        private string GetRoleTypeName(int messageId)
        {
            switch ((MsgCode)messageId)
            {
                case MsgCode.PositionTakingMastersExceedSuper:
                case MsgCode.PositionTakingMastersExceedMaster:
                    return Lang.Masters;

                case MsgCode.PositionTakingAgentsExceedMaster:
                case MsgCode.PositionTakingAgentsExceedAgent:
                    return Lang.Agents;

                case MsgCode.PositionTakingMembersExceedSuper:
                case MsgCode.PositionTakingMembersExceedMaster:
                case MsgCode.PositionTakingMembersExceedAgent:
                    return Lang.Members;

                default:
                    return string.Empty;
            }
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