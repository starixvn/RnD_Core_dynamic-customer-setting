namespace Fanex.CustomerSetting.SettingType15.Services
{
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;

    internal class MessageService : MessageServiceBase
    {
        public MessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }
    }
}