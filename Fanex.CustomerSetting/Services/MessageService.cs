namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Criteria;
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
        /// <param name="criteria">The update criteria.</param>
        /// <param name="model">The update model.</param>
        /// <returns>
        /// Returns translated message.
        /// </returns>
        public string GetTranslatedMessage(UpdateCriteria criteria, UpdateInfo model)
        {
            if (criteria.ErrCode == Result.SuccessValue)
            {
                return _successMsgs[model.SettingType];
            }

            switch ((MsgType)criteria.MsgType)
            {
                case MsgType.Simple:
                    return TranslateSimpleMessage(criteria, model);

                case MsgType.Status:
                case MsgType.Discount:
                case MsgType.BetLimit:
                case MsgType.PositionTaking:
                    return TranslateTemplateMessage<NormalMessage, SimpleRow>(criteria, model);

                case MsgType.PositionTakingDownline:
                    return TranslateTemplateMessage<PositionTakingDownlineMessage, PositionTakingDownlineRow>(criteria, model);

                case MsgType.PositionTakingMin:
                    return TranslateTemplateMessage<PositionTakingMinMessage, PositionTakingMinRow>(criteria, model);

                case MsgType.PositionTakingMax:
                    return TranslateTemplateMessage<PositionTakingMaxMessage, PositionTakingMaxRow>(criteria, model);

                case MsgType.PositionTakingMinMax:
                    return TranslateTemplateMessage<PositionTakingMinMaxMessage, PositionTakingMinMaxRow>(criteria, model);

                case MsgType.Other:
                    return criteria.ErrMsg;
            }

            return criteria.ErrMsg;
        }

        private string TranslateSimpleMessage(UpdateCriteria criteria, UpdateInfo model)
        {
            var simpleMessage = Deserialize<SimpleMessage>(criteria.ErrMsg);
            return TranslateResource(simpleMessage.MessageId, model.SettingType);
        }

        /// <summary>
        /// Translates the template message.
        /// </summary>
        /// <typeparam name="M">Type of template message.</typeparam>
        /// <typeparam name="R">Type of message row.</typeparam>
        /// <param name="criteria">The update criteria.</param>
        /// <param name="model">The update model.</param>
        /// <returns>Returns translate message.</returns>
        private string TranslateTemplateMessage<M, R>(UpdateCriteria criteria, UpdateInfo model)
            where M : TemplateMessage<R>
            where R : SimpleRow
        {
            var templateMessage = Deserialize<M>(criteria.ErrMsg);
            var message = TranslateResource(templateMessage.MessageId, model.SettingType);
            if (model.IsMultiple)
            {
                message = string.Format("{0}: {1}", model.UserName, message);
            }

            var stringBuilder = new StringBuilder(message);
            if (typeof(R) == typeof(PositionTakingDownlineRow))
            {
            }
            else
            {
                foreach (var row in templateMessage.Rows)
                {
                    stringBuilder.AppendFormat("<br />{0}", row.ToString(_resourceService));
                }
            }

            return stringBuilder.ToString();
        }

        private string TranslateResource(int messageId, SettingType settingType)
        {
            var prefix = _prefixs[settingType];
            var key = string.Format("{0}_{1}", prefix, messageId);
            return Error.ResourceManager.GetString(key);
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