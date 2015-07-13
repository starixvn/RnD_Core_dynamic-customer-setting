namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class GetPTMatrixCriteria : CriteriaBase
    {
        public byte ProductId { get; set; }

        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "CustSetting_PTMatrix";
        }

        public override bool IsValid()
        {
            return ProductId > 0 && !string.IsNullOrEmpty(UserName);
        }
    }
}