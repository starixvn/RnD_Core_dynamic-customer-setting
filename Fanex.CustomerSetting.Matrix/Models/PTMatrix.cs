namespace Fanex.CustomerSetting.Matrix.Models
{
    using System.Collections.Generic;

    public class PTMatrix : BaseMatrix
    {
        public bool IsLive { get; set; }

        public IEnumerable<PTMatrixItem> PTMatrixItems { get; set; }
    }
}