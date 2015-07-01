namespace Fanex.CustomerSetting.SettingType3.Template1.Services
{
    using System.Resources;
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;
    using Fanex.CustomerSetting.SettingType3.Template1.Resources;

    public class CommissionMessageService : BaseMessageService, IMessageService
    {
        private readonly ResourceManager _resourceManager = CommissionMessage.ResourceManager;

        public CommissionMessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public override MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language)
        {
            switch (msgType)
            {
                case SettingType3MessageType.CommissionViolation:
                    return TranslateDefaultSettingTypeViolationMessage(_resourceManager, rawMessage, settingLevel, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }
    }
}