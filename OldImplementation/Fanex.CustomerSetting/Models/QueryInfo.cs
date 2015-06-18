namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class QueryInfo
    {
        public QueryInfo()
        {
        }

        public QueryInfo(QueryInfo queryInfo)
        {
            this.CustId = queryInfo.CustId;
            this.RoleId = queryInfo.RoleId;
            this.TargetCustId = queryInfo.TargetCustId;
            this.TargetRoleId = queryInfo.TargetRoleId;
            this.ProductId = queryInfo.ProductId;
            this.FormType = queryInfo.FormType;
            this.Language = queryInfo.Language;
        }

        public int CustId { get; set; }

        public RoleType RoleId { get; set; }

        public int TargetCustId { get; set; }

        public RoleType TargetRoleId { get; set; }

        public byte ProductId { get; set; }

        public FormType FormType { get; set; }

        public string Language { get; set; }
    }
}