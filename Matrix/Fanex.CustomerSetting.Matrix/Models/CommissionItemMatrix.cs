namespace Fanex.CustomerSetting.Matrix.Models
{
    using System.Collections.Generic;

    public class CommissionItemMatrix : SettingItemMatrix
    {
        public IEnumerable<CommissionItemMatrixRow> CommissionItemMatrixRows { get; set; }
    }
}