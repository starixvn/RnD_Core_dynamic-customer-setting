namespace Fanex.CustomerSetting.Matrix.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Enumerations;

    public class CommissionMatrixItem
    {
        public CustomerLevel CustomerLevel { get; set; }

        public decimal CommRate { get; set; }
    }
}
