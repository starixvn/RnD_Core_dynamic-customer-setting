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
                this.Language = info.GetString("Language");
                this.IP = info.GetString("IP");
                this.Domain = info.GetString("Domain");

                this.ProductId = info.GetInt32("ProductId");
                this.SettingTypeId = info.GetInt32("SettingTypeId");
                this.SettingTypeName = info.GetString("SettingTypeName");
                this.SettingTypeTemplate = info.GetInt32("SettingTypeId");

                this.ActorId = info.GetInt32("ActorId");
                this.ActorName = info.GetString("ActorName");
                this.ActorLevel = info.GetString("ActorLevel");
                this.SubAccountId = info.GetInt32("SubAccountId");

                this.CustomerId = info.GetInt32("CustomerId");
                this.CustomerIds = info.GetString("CustomerIds");
                this.CustomerLevel = info.GetString("CustomerLevel");
                this.UplineLevel = info.GetString("UplineLevel");
            }
        }

        //// System information

        public int SystemId { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public string Domain { get; set; }

        //// Product information
        
        public int ProductId { get; set; }

        public int SettingTypeId { get; set; }

        public string SettingTypeName { get; set; }

        public int SettingTypeTemplate { get; set; }

        //// Actor information

        public int ActorId { get; set; }

        public string ActorName { get; set; }

        public string ActorLevel { get; set; }

        public int SubAccountId { get; set; }

        //// Customer information

        public int CustomerId { get; set; }

        public string CustomerIds { get; set; }

        public string CustomerLevel { get; set; }

        public string UplineLevel { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                info.AddValue("SystemId", this.SystemId);
                info.AddValue("Language", this.Language);
                info.AddValue("Domain", this.Domain);
                info.AddValue("IP", this.IP);

                info.AddValue("ProductId", this.ProductId);
                info.AddValue("SettingTypeId", this.SettingTypeId);
                info.AddValue("SettingTypeName", this.SettingTypeName);
                info.AddValue("SettingTypeTemplate", this.SettingTypeTemplate);

                info.AddValue("ActorId", this.ActorId);
                info.AddValue("ActorName", this.ActorName);
                info.AddValue("ActorLevel", this.ActorLevel);
                info.AddValue("SubAccountId", this.SubAccountId);

                info.AddValue("CustomerId", this.CustomerId);
                info.AddValue("CustomerIds", this.CustomerIds);
                info.AddValue("CustomerLevel", this.CustomerLevel);
                info.AddValue("UplineLevel", this.UplineLevel);
            }
        }
    }
}
