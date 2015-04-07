namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// IFormHandler interface.
    /// </summary>
    public interface IFormHandler
    {
        bool HasPermission(QueryInfo model);
    }
}