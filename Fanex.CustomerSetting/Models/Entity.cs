namespace Fanex.CustomerSetting.Models
{
    using System;

    [Serializable]
    public abstract class Entity<TId>
    {
        public TId Id { get; set; }
    }
}
