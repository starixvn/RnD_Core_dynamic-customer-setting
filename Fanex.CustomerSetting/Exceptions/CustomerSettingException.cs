namespace Fanex.CustomerSetting.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CustomerSettingException : Exception
    {
        public CustomerSettingException()
        {
        }

        public CustomerSettingException(string message)
            : base(message)
        {
        }

        public CustomerSettingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CustomerSettingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.SystemId = info.GetInt32("SystemId");
                this.ProductId = info.GetInt32("ProductId");
                this.SettingTypeId = info.GetInt32("SettingTypeId");
                this.ActorId = info.GetInt32("ActorId");
                this.ActorLevel = info.GetInt32("ActorLevel");
                this.CustomerId = info.GetInt32("CustomerId");
                this.CustomerLevel = info.GetInt32("CustomerLevel");
                this.SubAccountId = info.GetInt32("SubAccountId");
            }
        }

        public int SystemId { get; set; }

        public int ProductId { get; set; }

        public int SettingTypeId { get; set; }

        public int ActorId { get; set; }

        public int ActorLevel { get; set; }

        public int CustomerId { get; set; }

        public int CustomerLevel { get; set; }

        public int SubAccountId { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                info.AddValue("SystemId", this.SystemId);
                info.AddValue("ProductId", this.ProductId);
                info.AddValue("SettingTypeId", this.SettingTypeId);
                info.AddValue("ActorId", this.ActorId);
                info.AddValue("ActorLevel", this.ActorLevel);
                info.AddValue("CustomerId", this.CustomerId);
                info.AddValue("CustomerLevel", this.CustomerLevel);
                info.AddValue("SubAccountId", this.SubAccountId);
            }
        }
    }
}
