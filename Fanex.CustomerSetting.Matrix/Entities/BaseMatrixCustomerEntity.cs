namespace Fanex.CustomerSetting.Matrix.Entities
{
    public abstract class BaseMatrixCustomerEntity
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public byte RoleId { get; set; }

        public int Currency { get; set; }

        public int SuperId { get; set; }

        public string SuperName { get; set; }

        public int MasterId { get; set; }

        public string MasterName { get; set; }

        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public byte SettingLevel { get; set; }
    }
}