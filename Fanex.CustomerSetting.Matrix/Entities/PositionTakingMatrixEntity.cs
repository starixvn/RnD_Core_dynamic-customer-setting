namespace Fanex.CustomerSetting.Matrix.Entities
{
    using System.Collections.Generic;

    public class PositionTakingMatrixEntity
    {
        public PositionTakingMatrixCustomerEntity Customer { get; set; }

        public IEnumerable<PositionTakingMatrixSettingEntity> Settings { get; set; }
    }
}