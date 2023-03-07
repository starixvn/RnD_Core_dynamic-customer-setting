namespace Fanex.CustomerSetting.SettingType22.Models
{
    public class CurrentWinLossAmountItem
    {
        public int SportType { get; set; }

        public string SportTypeName { get; set; }

        public decimal CurrentAmount { get; set; }

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

#pragma warning disable S1067 // Expressions should not be too complex
            return Equals(CurrentAmount, otherObj.CurrentAmount)
                && Equals(WinPercent, otherObj.WinPercent)
                && Equals(MaxWinAmount, otherObj.MaxWinAmount)
                && Equals(LossPercent, otherObj.LossPercent)
                && Equals(MaxLossAmount, otherObj.MaxLossAmount);
#pragma warning restore S1067 // Expressions should not be too complex
        }

        public override int GetHashCode()
        {
            return CurrentAmount.GetHashCode()
                ^ WinPercent.GetHashCode()
                ^ MaxWinAmount.GetHashCode()
                ^ LossPercent.GetHashCode()
                ^ MaxLossAmount.GetHashCode();
        }
    }
}