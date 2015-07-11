namespace Fanex.CustomerSetting.Dtos.Params
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class UpdatingMultipleSettingParam : UpdatingSettingParam
    {
        private IEnumerable<int> _customerIds;
        private IEnumerable<string> _customerNames;

        public UpdatingMultipleSettingParam(
            ISetting setting,
            IEnumerable<int> customerIds,
            IEnumerable<string> customerNames,
            CustomerLevel? uplineLevel)
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
                    throw new ArgumentOutOfRangeException("value", "CustomerIds cannot be null when updating multiple setting.");
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
                    throw new ArgumentOutOfRangeException("value", "CustomerNames cannot be null when updating multiple setting.");
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