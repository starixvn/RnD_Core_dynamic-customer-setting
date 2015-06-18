namespace Fanex.CustomerSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;

    public interface IResourceRepository
    {
        IEnumerable<ProductResourceEntity> GetProductResources(ProductResourceQueryCriteria criteria);
    }
}