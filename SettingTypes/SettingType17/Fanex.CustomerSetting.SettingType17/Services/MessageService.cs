namespace Fanex.CustomerSetting.SettingType17.Services
{
    using Constants;
    using CustomerSetting.Models.Messages;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType17.Models.Messages;
    using Resources;
    using System.Resources;

    internal class MessageService : MessageServiceBase, IMessageService
    {
        public MessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public override MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language)
        {
#pragma warning disable S1301 // "switch" statements should have at least 3 "case" clauses
            switch (msgType)
#pragma warning restore S1301 // "switch" statements should have at least 3 "case" clauses
            {
                case MessageTypes.StakeLevelViolation:
                    return TranslateViolationMessage(Message.ResourceManager, rawMessage, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }

        internal MessageContent TranslateViolationMessage(
            ResourceManager resouceManager,
            string rawMessage,
            string language)
        {
            var mainMessage = Deserialize<MainMessage<ViolateStakeLevelUpdateSubMessage>>(rawMessage);

            var translatedMainMessage = string.Format(
                GetMessage(resouceManager, mainMessage.MessageId), 
                mainMessage.SubMessages[0].MaxChoice, 
                ResourceService.GetSportTypeName(mainMessage.SubMessages[0].SportType, language));

            return new MessageContent(translatedMainMessage);
        }
    }
}