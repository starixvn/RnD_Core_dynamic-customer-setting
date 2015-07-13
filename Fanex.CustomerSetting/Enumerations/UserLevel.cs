namespace Fanex.CustomerSetting.Enumerations
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Constants;

    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Reviewed")]
    public class UserLevel : Enumeration
    {
        public static readonly UserLevel Company = new UserLevel(Level.Company, "Company");
        public static readonly UserLevel Super = new UserLevel(Level.Super, "Super");
        public static readonly UserLevel Master = new UserLevel(Level.Master, "Master");
        public static readonly UserLevel Agent = new UserLevel(Level.Agent, "Agent");

        public UserLevel() { }
        private UserLevel(int value, string displayName) : base(value, displayName) { }

        public bool IsDirectUplineLevelOf(int downlineLevelValue)
        {
            return GapBetweenTwoLevels(this.Value, downlineLevelValue) == 1;
        }
        public bool IsDirectUplineLevelOf(CustomerLevel customerLevel)
        {
            return IsDirectUplineLevelOf((int)customerLevel);
        }

        public bool IsDirectDownlineLevelOf(int uplineLevelValue)
        {
            return GapBetweenTwoLevels(uplineLevelValue, this.Value) == 1;
        }

        private static int GapBetweenTwoLevels(int uplineLevelValue, int downlineLevelValue)
        {
            return uplineLevelValue - downlineLevelValue;
        }
    }
}