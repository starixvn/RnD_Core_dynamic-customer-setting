namespace Fanex.CustomerSetting.Casino.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CustomerCasinoStatus
    {
        public bool IsSynced { get; set; }

        public bool IsDownlineSynced { get; set; }

        public bool IsHidden { get; set; }
    }
}
