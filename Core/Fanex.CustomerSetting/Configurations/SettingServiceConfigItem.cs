namespace Fanex.CustomerSetting.Configurations
{
    using System;
    using System.Collections.Generic;
    using Common.Helper;

    internal class SettingServiceConfigItem
    {
        private Type queryServiceType;
        private Type updateServiceType;
        private List<byte> systems;
        private List<byte> productIds;

        internal SettingServiceConfigItem(
            Type queryServiceType,
            Type updateServiceType,
            List<byte> systems = null,
            List<byte> productIds = null)
        {
            Check.Argument.IsNotNull(queryServiceType, "queryServiceType");
            Check.Argument.IsNotNull(updateServiceType, "updateServiceType");

            this.queryServiceType = queryServiceType;
            this.updateServiceType = updateServiceType;
            this.systems = systems ?? new List<byte>();
            this.productIds = productIds ?? new List<byte>();
        }

        public Type QueryServiceType
        {
            get { return queryServiceType; }
        }

        public Type UpdateServiceType
        {
            get { return updateServiceType; }
        }

        public List<byte> Systems
        {
            get { return systems; }
        }

        public List<byte> ProductIds
        {
            get { return productIds; }
        }

        public virtual bool IsValid()
        {
            return UpdateServiceType != null && QueryServiceType != null;
        }

        public override bool Equals(object obj)
        {
            var that = obj as SettingServiceConfigItem;

            return that != null
                && this.QueryServiceType == that.QueryServiceType
                && this.UpdateServiceType == that.UpdateServiceType;
        }

        public override int GetHashCode()
        {
            return this.UpdateServiceType.GetHashCode() + this.QueryServiceType.GetHashCode();
        }
    }
}