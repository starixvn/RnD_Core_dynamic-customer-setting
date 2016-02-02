namespace Fanex.CustomerSetting.Matrix.Models
{
    using System.Collections.Generic;

    public class PTMatrix : SettingTypeMatrix
    {
        public bool IsAllowMasterAutoPT { get; set; }

        public IEnumerable<PTItemMatrix> PTItemMatrices { get; set; }
    }
}