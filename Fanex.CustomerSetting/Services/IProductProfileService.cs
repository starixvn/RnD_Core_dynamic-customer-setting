namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// IProductProfileService class.
    /// </summary>
    public interface IProductProfileService
    {
        ProductProfileEntity GetProductProfile(byte productId);
    }
}