namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.NAP.Framework.Data;

    public class CustomerService : ICustomerService
    {
        private readonly IDynamicRepository _dynamicRepository;

        public CustomerService(IDynamicRepository dynamicRepository = null)
        {
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public IEnumerable<Customer> GetCustomerLine(string customerName)
        {
            var custtomerDtos = _dynamicRepository.Fetch<CustomerLineDto>(new CustomerLineQueryCriteria
                {
                    UserName = customerName
                });

            IEnumerable<Customer> customerLine = new List<Customer>();

            return customerLine;
        }
    }
}