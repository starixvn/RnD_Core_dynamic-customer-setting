namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class QueryInfo
    {
        public int CustId { get; set; }

        public RoleType RoleId { get; set; }

        public int TargetCustId { get; set; }

        public RoleType TargetRoleId { get; set; }

        public byte ProductId { get; set; }

        public FormType FormType { get; set; }

        public string Language { get; set; }
    }
}