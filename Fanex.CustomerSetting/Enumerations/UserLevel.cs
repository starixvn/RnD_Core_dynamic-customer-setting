namespace Fanex.CustomerSetting.Enumerations
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Constants;

    [Serializable]
    public class UserLevel : Enumeration
    {
        public static readonly UserLevel Company = new UserLevel(Level.Company, "Company");
        public static readonly UserLevel Super = new UserLevel(Level.Super, "Super");
        public static readonly UserLevel Master = new UserLevel(Level.Master, "Master");
        public static readonly UserLevel Agent = new UserLevel(Level.Agent, "Agent");
        public static readonly UserLevel Member = new UserLevel(Level.Member, "Member");

        public UserLevel()
        {
        }

        private UserLevel(byte value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsMemberLevel
        {
            get
            {
                return this == Member;
            }
        }

        public bool IsNotMemberLevel
        {
            get
            {
                return !IsMemberLevel;
            }
        }

        public bool IsCompanyLevel
        {
            get
            {
                return this == Company;
            }
        }

        public bool IsNotCompanyLevel
        {
            get
            {
                return !IsCompanyLevel;
            }
        }

        public bool IsDirectUplineLevelOf(byte downlineLevelValue)
        {
            return GapBetweenTwoLevels(this.Value, downlineLevelValue) == 1;
        }

        public bool IsDirectUplineLevelOf(UserLevel customerLevel)
        {
            return IsDirectUplineLevelOf(customerLevel.Value);
        }

        public bool IsDirectDownlineLevelOf(byte uplineLevelValue)
        {
            return GapBetweenTwoLevels(uplineLevelValue, this.Value) == 1;
        }

        public bool Is(Enumeration level)
        {
            if (level != null)
            {
                return this == level;
            }

            return false;
        }

        public bool IsNot(Enumeration level)
        {
            return !Is(level);
        }

        private static int GapBetweenTwoLevels(byte uplineLevelValue, byte downlineLevelValue)
        {
            return uplineLevelValue - downlineLevelValue;
        }
    }
}