namespace Fanex.CustomerSetting.Models
{
    using System;

    [Serializable]
#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
    public abstract class Entity<TId>
#pragma warning restore S1694 // An abstract class should have both abstract and concrete methods
    {
        public TId Id { get; set; }
    }
}