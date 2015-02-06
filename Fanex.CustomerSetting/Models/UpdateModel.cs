namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// UpdateModel class.
    /// </summary>
    public class UpdateModel
    {
        public int CustId { get; set; }

        public RoleType RoleId { get; set; }

        public int TargetCustId { get; set; }

        public RoleType TargetRoleId { get; set; }

        public int SubAccountId { get; set; }

        public int ProductId { get; set; }

        public FormType FormType { get; set; }

        public SettingType SettingType { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }
    }
}