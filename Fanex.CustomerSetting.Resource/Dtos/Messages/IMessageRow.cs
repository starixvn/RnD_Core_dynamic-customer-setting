namespace Fanex.CustomerSetting.Resource.Dtos.Messages
{
    using Fanex.CustomerSetting.Resource.Services;

    public interface IMessageRow
    {
        string ToString(IResourceService resourceService, string language, byte settingLevel);
    }
}