namespace Fanex.CustomerSetting.SettingType14.Dtos
{
    internal class TicketCommissionDto
    {
        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public decimal? MaxGivenCommission { get; set; }

        public decimal? SuperCommission { get; set; }

        public decimal? MasterCommission { get; set; }

        public decimal? AgentCommission { get; set; }

        public decimal? MemberCommission { get; set; }
    }
}