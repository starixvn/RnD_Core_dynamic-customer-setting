namespace Fanex.CustomerSetting.Repositories
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.Data;

    /// <summary>
    /// ProductProfileRepository class.
    /// </summary>
    public class ProductProfileRepository : IProductProfileRepository
    {
        public ProductProfileEntity GetProductProfile(ProductProfileSelectCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var items = objectDb.Query<ProductProfileItemEntity>(criteria.GetSpParams());
                return new ProductProfileEntity
                {
                    Items = items
                };
            }
        }
    }
}