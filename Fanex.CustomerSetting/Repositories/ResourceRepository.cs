namespace Fanex.CustomerSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.Data;

    /// <summary>
    /// ResourceRepository class.
    /// </summary>
    public class ResourceRepository : IResourceRepository
    {
        public IEnumerable<ProductResourceEntity> GetProductResources(ProductResourceQueryCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDb.Query<ProductResourceEntity>(criteria.GetSpParams());
            }
        }
    }
}