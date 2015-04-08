namespace Fanex.CustomerSetting.Core
{
    /// <summary>
    /// Constants class.
    /// </summary>
    public static class Configuration
    {
        public const decimal MaxDiscountValue = 1m;
        public const decimal MaxBetLimitValue = 99999999999;
        public const byte MaxPositionTakingValue = 90;
        public const byte PositionTakingDivider = 100;
        public const int CacheTimeout = 12 * 60 * 60; // 1 day
        public const string ProductResourceCacheKey = "CSR_PRODUCT_RESOURCE";

        public static string GenerateProductProfileCacheKey(int productId)
        {
            return string.Format("CSR_PRODUCT_PROFILE_{0}", productId);
        }
    }
}