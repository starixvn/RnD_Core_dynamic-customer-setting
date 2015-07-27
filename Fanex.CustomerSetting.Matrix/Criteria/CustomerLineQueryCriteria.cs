namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class CustomerLineQueryCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_CustomerLineInfoSelect";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName);
        }
    }
}
