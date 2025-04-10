namespace Fanex.CustomerSetting.Matrix.Dtos
{
    public class CustomerLineDto
    {
        public int? CustId { get; set; }

        public string UserName { get; set; }

        public byte LevelValue { get; set; }

        public int? Currency { get; set; }

        public int? SuperId { get; set; }

        public int? MasterId { get; set; }

        public int? AgentId { get; set; }

        public string SuperName { get; set; }

        public string MasterName { get; set; }

        public string AgentName { get; set; }
    }
}