namespace Fanex.CustomerSetting.Dtos.Messages
{
    using Fanex.CustomerSetting.Services;

    public interface IMessageRow
    {
        string ToString(IResourceService resourceService, string language, byte settingLevel);
    }
}