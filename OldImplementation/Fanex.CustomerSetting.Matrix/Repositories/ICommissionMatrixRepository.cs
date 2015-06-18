namespace Fanex.CustomerSetting.Matrix.Repositories
{
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Entities;

    public interface ICommissionMatrixRepository
    {
        CommissionMatrixEntity Query(SettingMatrixQueryCriteria criteria);
    }
}