namespace Fanex.CustomerSetting.Core.Criteria.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class ProductResourceQueryCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_CustSetting_ProductNamesSelect";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
