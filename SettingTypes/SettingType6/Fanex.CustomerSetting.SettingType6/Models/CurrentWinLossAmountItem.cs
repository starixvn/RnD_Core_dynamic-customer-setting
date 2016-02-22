namespace Fanex.CustomerSetting.SettingType6.Models
{
    public class CurrentWinLossAmountItem
    {
        public decimal WinPercent { get; set; }

        public decimal MaxWinAmount { get; set; }

        public decimal LossPercent { get; set; }

        public decimal MaxLossAmount { get; set; }

        public override bool Equals(object obj)
        {
            var otherObj = obj as CurrentWinLossAmountItem;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return Equals(WinPercent, otherObj.WinPercent)
                    && Equals(MaxWinAmount, otherObj.MaxWinAmount)
                    && Equals(LossPercent, otherObj.LossPercent)
                    && Equals(MaxLossAmount, otherObj.MaxLossAmount);
            }
        }

        public override int GetHashCode()
        {
            return WinPercent.GetHashCode()
                ^ MaxWinAmount.GetHashCode()
                ^ LossPercent.GetHashCode()
                ^ MaxLossAmount.GetHashCode();
        }
    }
}