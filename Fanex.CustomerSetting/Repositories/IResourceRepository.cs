namespace Fanex.CustomerSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// IResourceRepository class.
    /// </summary>
    public interface IResourceRepository
    {
        IEnumerable<ProductResourceEntity> GetProductResources(ProductResourceSelectCriteria criteria);
    }
}