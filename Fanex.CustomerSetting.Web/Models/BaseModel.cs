namespace Fanex.CustomerSetting.Web.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public abstract class BaseModel
    {
        public int CustId { get; set; }

        public RoleType RoleId { get; set; }

        public int TargetCustId { get; set; }

        public string TargetCustName { get; set; }

        public RoleType TargetRoleId { get; set; }

        public byte ProductId { get; set; }

        public FormType FormType { get; set; }

        public string Language { get; set; }
    }
}