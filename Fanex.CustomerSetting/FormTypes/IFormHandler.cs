namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Models;

    public interface IFormHandler
    {
        bool IsCommandOnMember { get; }

        bool HasPermission(QueryInfo model);
    }
}