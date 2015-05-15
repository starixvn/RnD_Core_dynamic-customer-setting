namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    public interface IQueryService
    {
        Result Do(QueryInfo queryInfo);
    }
}