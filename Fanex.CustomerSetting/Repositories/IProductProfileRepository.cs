namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// IProductProfileRepository class.
    /// </summary>
    public interface IProductProfileRepository
    {
        ProductProfileEntity GetProductProfile(ProductProfileQueryCriteria criteria);
    }
}