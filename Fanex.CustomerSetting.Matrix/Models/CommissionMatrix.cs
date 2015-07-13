namespace Fanex.CustomerSetting.Matrix.Models
{
    using System.Collections.Generic;

    public class CommissionMatrix : BaseMatrix
    {
        public IEnumerable<CommissionMatrixItem> CommissionMatrixItems { get; set; }
    }
}