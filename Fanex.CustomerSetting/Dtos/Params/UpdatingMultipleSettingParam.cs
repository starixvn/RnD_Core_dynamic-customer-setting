namespace Fanex.CustomerSetting.Dtos.Params
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UpdatingMultipleSettingParam : UpdatingSettingParam
    {
        private IEnumerable<int> _customerIds;
        private IEnumerable<string> _customerNames;

        public IEnumerable<int> CustomerIds
        {
            get
            {
                return _customerIds;
            }

            set
            {
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

                if (customerNamesLength <= 0)
                {
                    throw new ArgumentOutOfRangeException("CustomerIds", "CustomerIds should have at least one customer id.");
                }

                if (customerNamesLength <= 0)
                {
                    throw new ArgumentOutOfRangeException("CustomerNames", "CustomerNames should have at least one customer id.");
                }

                if (customerIdsLength != customerNamesLength)
                {
                    throw new ArgumentOutOfRangeException("CustomerIds", "CustomerIds does not match with CustomerNames");
                }
            }
        }
    }
}