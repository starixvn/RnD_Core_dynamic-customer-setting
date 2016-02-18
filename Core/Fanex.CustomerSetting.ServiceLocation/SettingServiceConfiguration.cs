namespace Fanex.CustomerSetting.ServiceLocation
{
    using System;

    public class SettingServiceConfiguration
    {
        public byte? System { get; set; }

        public byte ProductId { get; set; }

        public Type UpdateServiceType { get; set; }

        public Type QueryServiceType { get; set; }

        public virtual bool IsValid()
        {
            return (!System.HasValue || System.Value > 0)
                && ProductId > 0
                && UpdateServiceType != null
                && QueryServiceType != null;
        }

        ////public override bool Equals(object obj)
        ////{
        ////    var right = obj as SettingServiceConfiguration;

        ////    if (right != null)
        ////    {
        ////        return this.System == right.System
        ////            && this.ProductId == this.ProductId;
        ////    }

        ////    return false;
        ////}

        ////public override int GetHashCode()
        ////{
        ////    unchecked
        ////    {
        ////        // Choose large primes to avoid hashing collisions
        ////        const int HashingBase = (int)2166136261;
        ////        const int HashingMultiplier = 16777619;

        ////        int hash = HashingBase;
        ////        hash = (hash * HashingMultiplier) ^ ProductId.GetHashCode();
        ////        hash = (hash * HashingMultiplier) ^ (this.System.HasValue ? this.System.HasValue.GetHashCode() : this.System.GetHashCode());
        ////        return hash;
        ////    }
        ////}
    }
}