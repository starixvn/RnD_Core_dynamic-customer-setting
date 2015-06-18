namespace Fanex.CustomerSetting.Matrix.Services
{
    using Fanex.CustomerSetting.Matrix.Entities;
    using Fanex.CustomerSetting.Matrix.Models;

    public interface ICommissionMatrixService
    {
        CommissionMatrixEntity Query(QueryInfo queryInfo);
    }
}