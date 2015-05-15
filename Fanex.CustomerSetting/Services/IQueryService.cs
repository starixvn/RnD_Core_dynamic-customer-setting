namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    public interface IQueryService
    {
        Result Execute(QueryInfo queryInfo);
    }
}