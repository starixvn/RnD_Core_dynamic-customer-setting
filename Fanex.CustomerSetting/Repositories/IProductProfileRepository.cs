namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;

    public interface IProductProfileRepository
    {
        ProductProfileEntity GetProductProfile(ProductProfileQueryCriteria criteria);
    }
}