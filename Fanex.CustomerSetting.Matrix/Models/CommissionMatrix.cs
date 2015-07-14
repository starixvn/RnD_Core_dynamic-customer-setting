namespace Fanex.CustomerSetting.Matrix.Models
{
    using System.Collections.Generic;

    public class CommissionMatrix : SettingTypeMatrix
    {
        public IEnumerable<CommissionItemMatrix> CommissionItemMatrices { get; set; }
    }
}