namespace Fanex.CustomerSetting.Models.Requests
{
    using Enumerations;
    using Fanex.CustomerSetting.Configurations;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MultipleUpdateCustomerSettingCommand : UpdateCustomerSettingCommand
    {
        private IEnumerable<int> _customerIds;
        private IEnumerable<string> _customerNames;

        public MultipleUpdateCustomerSettingCommand(
            ISetting setting,
            IEnumerable<int> customerIds,
            IEnumerable<string> customerNames,
            Level uplineLevel,
            byte settingStatus = 0)
            : base(setting, settingStatus, uplineLevel)
        {
            CustomerIds = customerIds;
            CustomerNames = customerNames;
        }

        public IEnumerable<int> CustomerIds
        {
            get
            {
                return _customerIds;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "CustomerIds cannot be null when update multiple setting.");
                }

                ValidateCustomerIdListWithCustomerNameList(value, _customerNames);

                _customerIds = value;
            }
        }

        public IEnumerable<string> CustomerNames
        {
            get
            {
                return _customerNames;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "CustomerNames cannot be null when update multiple setting.");
                }

                ValidateCustomerIdListWithCustomerNameList(_customerIds, value);

                _customerNames = value;
            }
        }

        private void ValidateCustomerIdListWithCustomerNameList(IEnumerable<int> customerIds, IEnumerable<string> customerNames)
        {
            if (customerIds != null && customerNames != null)
            {
                int customerIdsLength = customerIds.Count();
                int customerNamesLength = customerNames.Count();

                if (customerIdsLength <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(customerIds), "CustomerIds should have at least one customer id.");
                }

                if (customerNamesLength <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(customerNames), "CustomerNames should have at least one customer id.");
                }

                if (customerIdsLength != customerNamesLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(customerIds), "CustomerIds does not match with CustomerNames");
                }
            }
        }

        public UpdateCustomerSettingCommand GetUpdateCustomerSettingCommand(int customerId)
        {
            int customerIdIndex = CustomerIds.ToList().IndexOf(customerId);

            if (customerIdIndex < 0)
            {
                throw new ArgumentException($"{customerId} does not exist in selected ids for update action");
            }

            var command = new UpdateCustomerSettingCommand(Setting, AppSettings.SettingStatus, UplineLevel);
            command.Setting.CustomerId = customerId;
            command.Setting.CustomerInfo.UserName = CustomerNames.ElementAt(customerIdIndex);

            return command;
        }
    }
}