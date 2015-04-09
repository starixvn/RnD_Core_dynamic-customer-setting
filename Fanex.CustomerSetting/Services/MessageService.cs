namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
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

            var messageBuilder = MessageBuilderFactory.Create(messageInfo.MsgType, _resourceService);
            return messageBuilder.Build(messageInfo);
        }
    }
}