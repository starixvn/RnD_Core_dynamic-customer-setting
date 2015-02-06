namespace Fanex.CustomerSetting.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// ProductProfileEntity class.
    /// </summary>
    public class ProductProfileEntity
    {
        public IEnumerable<ProductProfileItemEntity> Items { get; set; }
    }
}