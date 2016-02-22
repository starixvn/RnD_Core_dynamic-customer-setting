namespace Fanex.CustomerSetting.SettingType6.Models
{
    internal class CurrentWinLossAmountQuery
    {
        public int CustomerId { get; set; }

        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public override bool Equals(object obj)
        {
            var otherObj = obj as CurrentWinLossAmountQuery;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return Equals(CustomerId, otherObj.CustomerId)
                    && Equals(SportTypeId, otherObj.SportTypeId)
                    && Equals(BetTypeId, otherObj.BetTypeId);
            }
        }

        public override int GetHashCode()
        {
            return CustomerId.GetHashCode()
                ^ SportTypeId.GetHashCode()
                ^ BetTypeId.GetHashCode();
        }
    }
}