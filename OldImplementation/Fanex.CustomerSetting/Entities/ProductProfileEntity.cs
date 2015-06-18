namespace Fanex.CustomerSetting.Entities
{
    using System.Collections.Generic;

    public class ProductProfileEntity
    {
        public IEnumerable<ProductProfileItemEntity> Items { get; set; }
    }
}