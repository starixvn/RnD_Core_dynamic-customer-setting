namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Entities;

    public interface IProductProfileService
    {
        ProductProfileEntity GetProductProfile(byte productId);
    }
}