namespace Fanex.CustomerSetting.Models.Enumerations
{
    public static class SystemType
    {
        public const byte Nap = 1;
        public const byte Alpha = 2;
        public const byte InternalAlpha = 3;
        public const byte ExternalNap = 4;
        public const byte ExternalNapBetSetting = 5;
        public const byte Y7Alpha = 6;
        public const byte Y7InternalAlpha = 7;
        public const byte Y7ExternalNap = 8;
        public const byte Y7ExternalNapBetSetting = 9;

        public static bool IsExternalNap(byte systemId)
        {
            return ExternalNap == systemId
                || Y7ExternalNap == systemId
                || ExternalNapBetSetting == systemId
                || Y7ExternalNapBetSetting == systemId;
        }

        public static bool IsNotExternalNapBetSetting(byte systemId)
        {
            return ExternalNapBetSetting != systemId && Y7ExternalNapBetSetting != systemId;
        }

        public static bool IsAlpha(byte systemId)
        {
            return InternalAlpha == systemId
                || Alpha == systemId
                || Y7InternalAlpha == systemId
                || Y7Alpha == systemId;
        }

        public static bool IsY7(byte systemId)
        {
            return Y7InternalAlpha == systemId
                || Y7Alpha == systemId
                || Y7ExternalNap == systemId
                || Y7ExternalNapBetSetting == systemId;
        }
    }
}