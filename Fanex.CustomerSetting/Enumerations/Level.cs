namespace Fanex.CustomerSetting.Enumerations
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Constants;

    [Serializable]
    public class Level : Enumeration
    {
        public static readonly Level Company = new Level(5, "Company");
        public static readonly Level Super = new Level(4, "Super");
        public static readonly Level Master = new Level(3, "Master");
        public static readonly Level Agent = new Level(2, "Agent");
        public static readonly Level Member = new Level(1, "Member");

        public Level()
        {
        }

        private Level(byte value, string displayName)
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

        public bool IsDirectUplineLevelOf(Level customerLevel)
        {
            return IsDirectUplineLevelOf(customerLevel);
        }

        public bool IsDirectDownlineLevelOf(Level uplineLevel)
        {
            return GapBetweenTwoLevels(uplineLevel.Value, this.Value) == 1;
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