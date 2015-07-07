namespace Fanex.CustomerSetting.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;

    [Serializable]
    public class Setting : ISetting
    {
        public Setting()
        {
        }

        public Setting(
            int customerId, 
            CustomerLevel customerLevel, 
            CustomerInfo customerInfo, 
            byte productId,
            string productName,
            IEnumerable<SettingType> settingTypes)
        {
            CustomerId = customerId;
            CustomerLevel = customerLevel;
            CustomerInfo = customerInfo;
            ProductId = productId;
            ProductName = productName;
            SettingTypes = settingTypes;
        }

        public byte ProductId { get; set; }

        public string ProductName { get; private set; }

        public int CustomerId { get; set; }

        public CustomerLevel CustomerLevel { get; set; }

        public CustomerInfo CustomerInfo { get; private set; }

        public IEnumerable<SettingType> SettingTypes { get; set; }
    }
}