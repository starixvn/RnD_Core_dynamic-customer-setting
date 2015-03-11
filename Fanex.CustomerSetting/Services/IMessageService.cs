namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// IMessageService class.
    /// </summary>
    public interface IMessageService
    {
        string GetTranslatedMessage(UpdateCriteria criteria, UpdateInfo model);
    }
}