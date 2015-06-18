namespace Fanex.CustomerSetting.Matrix.Entities
{
    using System.Collections.Generic;

    public class CommissionMatrixEntity
    {
        public CommissionMatrixCustomerEntity Customer { get; set; }

        public IEnumerable<CommissionMatrixSettingEntity> Settings { get; set; }
    }
}