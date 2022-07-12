using System.Linq;
using Fanex.CustomerSetting.SettingType21.Models.Messages;

namespace Fanex.CustomerSetting.SettingType21.Services
{
    using Constants;
    using CustomerSetting.Models.Messages;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Resources;
    using System.Resources;

    internal class MessageService : MessageServiceBase, IMessageService
    {
        public MessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public override MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel,
            string language)
        {
#pragma warning disable S1301 // "switch" statements should have at least 3 "case" clauses
            switch (msgType)
#pragma warning restore S1301 // "switch" statements should have at least 3 "case" clauses
            {
                case MessageTypes.ProductStatusViolation:
                    return TranslateDefaultSettingTypeViolationMessage(Message.ResourceManager, rawMessage,
                        settingLevel, language);
                case MessageTypes.DependenceViolation:
                    return DependenceViolationMessageContent(Message.ResourceManager, rawMessage, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }

        public MessageContent DependenceViolationMessageContent(ResourceManager resourceManager, string rawMessage, string language)
        {
            var mainMessage = Deserialize<SettingType21MainMessage>(rawMessage);
          
            var templateMessage = GetMessage(resourceManager, mainMessage.MessageId);
            var messages = mainMessage.TranslateSubMessages(ResourceService, language, templateMessage);
                
            return new MessageContent("",messages);
        }
    }
}