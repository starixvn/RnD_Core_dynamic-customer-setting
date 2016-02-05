namespace Fanex.CustomerSetting.Casino.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.CustomerSetting.Models;

    public class CasinoSetting : Setting
    {
        public bool IsAllowedForceUpdate
        {
            get;
            set;
        }
    }
}
