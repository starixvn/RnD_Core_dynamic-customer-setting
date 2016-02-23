namespace Fanex.CustomerSetting.ServiceLocation
{
    using System;

    internal class SettingServiceConfigItem : ICloneable
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}