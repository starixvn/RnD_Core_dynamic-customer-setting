namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Constants;
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
            Check.Argument.IsNotEmpty(customerName, "customerName");

            var customerLineDto = GetCustomerLineDto(customerName);
            var customerLine = new List<Customer>();

            ////switch (customerLineDto.CustomerLevel.Value)
            ////{
            ////    case Level.Super:
            ////        customerLine.Add(GetCustomer(customerLineDto));

            ////        break;

            ////    case Level.Master:
            ////        customerLine.Add(GetSuper(customerLineDto));
            ////        customerLine.Add(GetCustomer(customerLineDto));

            ////        break;

            ////    case Level.Agent:
            ////        customerLine.Add(GetSuper(customerLineDto));
            ////        customerLine.Add(GetMaster(customerLineDto));
            ////        customerLine.Add(GetCustomer(customerLineDto));

            ////        break;

            ////    case Level.Member:
            ////        customerLine.Add(GetSuper(customerLineDto));
            ////        customerLine.Add(GetMaster(customerLineDto));
            ////        customerLine.Add(GetAgent(customerLineDto));
            ////        customerLine.Add(GetCustomer(customerLineDto));

            ////        break;
            ////}

            return customerLine;
        }

        private Customer GetSuper(CustomerLineDto customerLineDto)
        {
            var super = new Customer
            {
                Id = customerLineDto.SuperID.GetValueOrDefault(0),
                Name = customerLineDto.SuperName,
                Currency = customerLineDto.Currency.GetValueOrDefault(0),
                CustomerLevel = Level.Super
            };

            return super;
        }

        private Customer GetMaster(CustomerLineDto customerLineDto)
        {
            var master = new Customer
            {
                Id = customerLineDto.MasterID.GetValueOrDefault(0),
                Name = customerLineDto.MasterName,
                Currency = customerLineDto.Currency.GetValueOrDefault(0),
                CustomerLevel = Level.Master
            };

            return master;
        }

        private Customer GetAgent(CustomerLineDto customerLineDto)
        {
            var agent = new Customer
            {
                Id = customerLineDto.AgentID.GetValueOrDefault(0),
                Name = customerLineDto.AgentName,
                Currency = customerLineDto.Currency.GetValueOrDefault(0),
                CustomerLevel = Level.Agent
            };

            return agent;
        }

        private Customer GetCustomer(CustomerLineDto customerLineDto)
        {
            var customer = new Customer
            {
                Id = customerLineDto.CustID.GetValueOrDefault(0),
                Name = customerLineDto.UserName,
                Currency = customerLineDto.Currency.GetValueOrDefault(0),
                CustomerLevel = customerLineDto.CustomerLevel
            };

            return customer;
        }

        private CustomerLineDto GetCustomerLineDto(string customerName)
        {
            var customerLineDto = _dynamicRepository.Get<CustomerLineDto>(
                new CustomerLineQueryCriteria
                {
                    UserName = customerName
                });

            return customerLineDto;
        }
    }
}