namespace Fanex.CustomerSetting.SettingType12.Models
{
    public class CurrentLossAmountItem
    {
        public decimal LossPercent { get; set; }

        public decimal MaxLossAmount { get; set; }

        public override bool Equals(object obj)
        {
            var otherObj = obj as CurrentLossAmountItem;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return Equals(LossPercent, otherObj.LossPercent)
                    && Equals(MaxLossAmount, otherObj.MaxLossAmount);
            }
        }

        public override int GetHashCode()
        {
            return LossPercent.GetHashCode() ^ MaxLossAmount.GetHashCode();
        }
    }
}