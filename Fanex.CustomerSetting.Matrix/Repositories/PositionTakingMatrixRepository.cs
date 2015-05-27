namespace Fanex.CustomerSetting.Matrix.Repositories
{
    using System.Linq;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Entities;
    using Fanex.Data;

    public class PositionTakingMatrixRepository : IPositionTakingMatrixRepository
    {
        public PositionTakingMatrixEntity Query(SettingMatrixQueryCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customer = multi.Read<PositionTakingMatrixCustomerEntity>().First();
                    var settings = multi.Read<PositionTakingMatrixSettingEntity>();
                    return new PositionTakingMatrixEntity
                    {
                        Customer = customer,
                        Settings = settings
                    };
                }
            }
        }
    }
}