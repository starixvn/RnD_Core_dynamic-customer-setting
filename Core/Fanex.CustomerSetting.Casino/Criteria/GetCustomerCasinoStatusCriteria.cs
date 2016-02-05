namespace Fanex.CustomerSetting.Casino.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;

    public class GetCustomerCasinoStatusCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public byte ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_SyncStatusSelect";
        }

        public override bool IsValid()
        {
            return CustId > 0 && ProductId > 0;
        }
    }
}
