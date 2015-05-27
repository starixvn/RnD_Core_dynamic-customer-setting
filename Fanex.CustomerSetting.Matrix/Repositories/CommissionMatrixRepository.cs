namespace Fanex.CustomerSetting.Matrix.Repositories
{
    using System.Linq;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Entities;
    using Fanex.Data;

    public class CommissionMatrixRepository : ICommissionMatrixRepository
    {
        public CommissionMatrixEntity Query(SettingMatrixQueryCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customer = multi.Read<CommissionMatrixCustomerEntity>().First();
                    var settings = multi.Read<CommissionMatrixSettingEntity>();
                    return new CommissionMatrixEntity
                    {
                        Customer = customer,
                        Settings = settings
                    };
                }
            }
        }
    }
}