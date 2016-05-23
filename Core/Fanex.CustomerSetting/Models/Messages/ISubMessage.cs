namespace Fanex.CustomerSetting.Models.Messages
{
    using Services;

    public interface ISubMessage
    {
        string ToString(IResourceService resourceService, string language, byte settingLevel);
    }
}