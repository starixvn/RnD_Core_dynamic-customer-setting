namespace Fanex.CustomerSetting.Matrix.Models
{
    using System.Collections.Generic;

    public class PTItemMatrix : SettingItemMatrix
    {
        public bool IsLive { get; set; }

        public IEnumerable<PTItemMatrixRow> PTItemMatrixRows { get; set; }
    }
}