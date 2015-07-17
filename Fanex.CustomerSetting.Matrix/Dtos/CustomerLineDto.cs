namespace Fanex.CustomerSetting.Matrix.Dtos
{
    using Fanex.CustomerSetting.Enumerations;

    public class CustomerLineDto
    {
        public int? CustID { get; set; }

        public string UserName { get; set; }

        public UserLevel CustomerLevel { get; set; }

        public byte? Currency { get; set; }

        public int? SuperID { get; set; }

        public int? MasterID { get; set; }

        public int? AgentID { get; set; }

        public string SuperName { get; set; }

        public string MasterName { get; set; }

        public string AgentName { get; set; }
    }
}