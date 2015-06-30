namespace Fanex.CustomerSetting.Dtos.Messages
{
    using Fanex.CustomerSetting.Services;

    public interface ISubMessage
    {
        string ToString(IResourceService resourceService, string language, byte settingLevel);
    }
}