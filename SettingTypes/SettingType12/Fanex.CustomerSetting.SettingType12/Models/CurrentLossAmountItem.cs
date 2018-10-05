namespace Fanex.CustomerSetting.SettingType12.Models
{
    public class CurrentLossAmountItem
    {
        public decimal CurrentAmount { get; set; }

        public decimal LossPercent { get; set; }

        public decimal MaxLossAmount { get; set; }

        public override bool Equals(object obj)
        {
            var otherObj = obj as CurrentLossAmountItem;

            if (otherObj == null)
            {
                return false;
            }

            return Equals(CurrentAmount, otherObj.CurrentAmount)
                && Equals(LossPercent, otherObj.LossPercent)
                && Equals(MaxLossAmount, otherObj.MaxLossAmount);
        }

        public override int GetHashCode()
        {
            return CurrentAmount.GetHashCode()
                ^ LossPercent.GetHashCode()
                ^ MaxLossAmount.GetHashCode();
        }
    }
}