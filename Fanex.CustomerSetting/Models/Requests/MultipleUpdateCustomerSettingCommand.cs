namespace Fanex.CustomerSetting.Models.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;

    public class MultipleUpdateCustomerSettingCommand : UpdateCustomerSettingCommand
    {
        private IEnumerable<int> _customerIds;
        private IEnumerable<string> _customerNames;

        public MultipleUpdateCustomerSettingCommand()
        {
        }

        public MultipleUpdateCustomerSettingCommand(
            ISetting setting,
            IEnumerable<int> customerIds,
            IEnumerable<string> customerNames,
            Level uplineLevel)
            : base(setting, uplineLevel)
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
                    throw new ArgumentNullException("value", "CustomerIds cannot be null when update multiple setting.");
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
                    throw new ArgumentNullException("value", "CustomerNames cannot be null when update multiple setting.");
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
                    throw new ArgumentOutOfRangeException("customerIds", "CustomerIds should have at least one customer id.");
                }

                if (customerNamesLength <= 0)
                {
                    throw new ArgumentOutOfRangeException("customerNames", "CustomerNames should have at least one customer id.");
                }

                if (customerIdsLength != customerNamesLength)
                {
                    throw new ArgumentOutOfRangeException("customerIds", "CustomerIds does not match with CustomerNames");
                }
            }
        }
    }
}