namespace Fanex.CustomerSetting.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;

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
                this.ActionName = info.GetString("ActionName");

                this.SystemId = info.GetInt32("SystemId");
                this.Language = info.GetString("Language");
                this.IP = info.GetString("IP");
                this.Domain = info.GetString("Domain");

                this.ActorId = info.GetInt32("ActorId");
                this.ActorName = info.GetString("ActorName");
                this.ActorLevel = info.GetString("ActorLevel");
                this.SubAccountId = info.GetInt32("SubAccountId");

                this.ProductId = info.GetInt32("ProductId");
                this.SettingTypeId = info.GetInt16("SettingTypeId");
                this.SettingTypeName = info.GetString("SettingTypeName");
                this.SettingTypeTemplate = info.GetByte("SettingTypeId");
                this.SettingTypeLevel = info.GetByte("SettingTypeLevel");

                this.CustomerId = info.GetInt32("CustomerId");
                this.CustomerIds = info.GetString("CustomerIds");
                this.CustomerLevel = info.GetString("CustomerLevel");
                this.UplineLevel = info.GetString("UplineLevel");
            }
        }

        public string OriginalMessage 
        { 
            get
            {
                return base.Message;
            }
        }

        public string ActionName { get; set; }

        //// System information

        public int SystemId { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public string Domain { get; set; }

        //// Actor information

        public int ActorId { get; set; }

        public string ActorName { get; set; }

        public string ActorLevel { get; set; }

        public int SubAccountId { get; set; }

        //// Product information

        public int ProductId { get; set; }

        public short SettingTypeId { get; set; }

        public string SettingTypeName { get; set; }

        public byte SettingTypeLevel { get; set; }

        public byte SettingTypeTemplate { get; set; }

        //// Customer information

        public int CustomerId { get; set; }

        public string CustomerIds { get; set; }

        public string CustomerLevel { get; set; }

        public string UplineLevel { get; set; }

        public override string Message
        {
            get
            {
                return BuildMessage();
            }
        }

        public void SetSettingContextInfo(string actionName, ISettingContext settingContext)
        {
            if (settingContext == null)
            {
                throw new ArgumentNullException("settingContext", "Setting Context cannot be null");
            }

            ActionName = actionName;

            SystemId = settingContext.System;
            Language = settingContext.Language;
            IP = settingContext.IP;
            Domain = settingContext.Domain;

            ActorId = settingContext.Actor.Id;
            ActorName = settingContext.Actor.Name;
            ActorLevel = settingContext.Actor.Level.DisplayName;
            SubAccountId = settingContext.Actor.SubAccountId;
        }

        public void SetProductInfo(byte productId, SettingType settingType = null)
        {
            ProductId = productId;

            if (settingType != null)
            {
                SettingTypeId = settingType.Id;
                SettingTypeName = settingType.Name;
                SettingTypeTemplate = settingType.SettingTemplate;
                SettingTypeLevel = settingType.SettingLevel;
            }
        }

        public void SetCustomerInfo(int customerId, Level customerLevel, Level uplineLevel = null)
        {
            CustomerId = customerId;

            SetCustomerLevelAndUplineLevel(customerLevel, uplineLevel);
        }

        public void SetCustomerInfo(string customerIds, Level customerLevel, Level uplineLevel = null)
        {
            CustomerIds = customerIds;

            SetCustomerLevelAndUplineLevel(customerLevel, uplineLevel);
        }

        private void SetCustomerLevelAndUplineLevel(Level customerLevel, Level uplineLevel)
        {
            CustomerLevel = customerLevel.DisplayName;

            if (uplineLevel != null)
            {
                UplineLevel = uplineLevel.DisplayName;
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                info.AddValue("ActionName", this.ActionName);

                info.AddValue("SystemId", this.SystemId);
                info.AddValue("Language", this.Language);
                info.AddValue("Domain", this.Domain);
                info.AddValue("IP", this.IP);

                info.AddValue("ActorId", this.ActorId);
                info.AddValue("ActorName", this.ActorName);
                info.AddValue("ActorLevel", this.ActorLevel);
                info.AddValue("SubAccountId", this.SubAccountId);

                info.AddValue("ProductId", this.ProductId);
                info.AddValue("SettingTypeId", this.SettingTypeId);
                info.AddValue("SettingTypeName", this.SettingTypeName);
                info.AddValue("SettingTypeTemplate", this.SettingTypeTemplate);
                info.AddValue("SettingTypeLevel", this.SettingTypeLevel);

                info.AddValue("CustomerId", this.CustomerId);
                info.AddValue("CustomerIds", this.CustomerIds);
                info.AddValue("CustomerLevel", this.CustomerLevel);
                info.AddValue("UplineLevel", this.UplineLevel);
            }
        }

        private string BuildMessage()
        {
            var messageBuilder = new StringBuilder();

            messageBuilder.AppendLine(base.Message);
            messageBuilder.AppendFormat("Action: {0}", ActionName).AppendLine();

            messageBuilder.AppendFormat("Language: {0}", Language).AppendLine();
            messageBuilder.AppendFormat("IP: {0}", IP).AppendLine();
            messageBuilder.AppendFormat("System: {0}", SystemId).AppendLine();
            messageBuilder.AppendFormat("Domain: {0}", Domain).AppendLine();

            messageBuilder.AppendFormat("Actor Id: {0}", ActorId).AppendLine();
            messageBuilder.AppendFormat("Actor Name: {0}", ActorName).AppendLine();
            messageBuilder.AppendFormat("Actor Level: {0}", ActorLevel).AppendLine();
            messageBuilder.AppendFormat("Sub Account Id: {0}", SubAccountId).AppendLine();

            messageBuilder.AppendFormat("Customer Id(s): {0}", CustomerId > 0 ? CustomerId.ToString() : CustomerIds).AppendLine();
            messageBuilder.AppendFormat("Customer Level: {0}", CustomerLevel).AppendLine();
            messageBuilder.AppendFormat("Upline Level: {0}", UplineLevel).AppendLine();

            messageBuilder.AppendFormat("ProductId: {0}", ProductId).AppendLine();

            messageBuilder.AppendFormat("Setting Type Id: {0}", SettingTypeId).AppendLine();
            messageBuilder.AppendFormat("Setting Type Name: {0}", SettingTypeName).AppendLine();
            messageBuilder.AppendFormat("Setting Type Level: {0}", SettingTypeLevel).AppendLine();
            messageBuilder.AppendFormat("Setting Type Template: {0}", SettingTypeTemplate).AppendLine();

            return messageBuilder.ToString();
        }
    }
}