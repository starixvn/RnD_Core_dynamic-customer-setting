namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    internal class CommissionMatrixQueryCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_CommMatrix";
        }

        public override bool IsValid()
        {
            return ProductId > 0 && !string.IsNullOrEmpty(UserName);
        }
    }
}