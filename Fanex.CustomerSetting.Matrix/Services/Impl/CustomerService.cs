namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Enumerations;
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
            var customerLineDto = _dynamicRepository.Get<CustomerLineDto>(new CustomerLineQueryCriteria
                {
                    UserName = customerName
                });

            switch (customerLineDto.CustomerLevel)
            {
                case CustomerLevel.Super:
                    break;

                case CustomerLevel.Master:
                    break;

                case CustomerLevel.Agent:
                    break;

                case CustomerLevel.Member:
                    break;
            }

            return Enumerable.Empty<Customer>();
        }
    }
}