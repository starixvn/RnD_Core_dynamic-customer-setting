namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.NAP.Framework.Data;

    internal class CustomerService : ICustomerService
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
            var customerLevel = Enumeration.FromValue<Level>((byte)customerLineDto.LevelValue);

            if (customerLevel.IsMasterLevel())
            {
                customerLine.Add(GetSuper(customerLineDto));
            }

            if (customerLevel.IsAgentLevel())
            {
                customerLine.Add(GetSuper(customerLineDto));
                customerLine.Add(GetMaster(customerLineDto));
            }

            if (customerLevel.IsMemberLevel())
            {
                customerLine.Add(GetSuper(customerLineDto));
                customerLine.Add(GetMaster(customerLineDto));
                customerLine.Add(GetAgent(customerLineDto));
            }

            customerLine.Add(GetCustomer(customerLineDto));

            return customerLine;
        }

        private Customer GetSuper(CustomerLineDto customerLineDto)
        {
            var super = new Customer
            {
                Id = customerLineDto.SuperId.GetValueOrDefault(0),
                Name = customerLineDto.SuperName,
                Currency = customerLineDto.Currency.GetValueOrDefault(0),
                Level = Level.Super
            };

            return super;
        }

        private Customer GetMaster(CustomerLineDto customerLineDto)
        {
            var master = new Customer
            {
                Id = customerLineDto.MasterId.GetValueOrDefault(0),
                Name = customerLineDto.MasterName,
                Currency = customerLineDto.Currency.GetValueOrDefault(0),
                Level = Level.Master
            };

            return master;
        }

        private Customer GetAgent(CustomerLineDto customerLineDto)
        {
            var agent = new Customer
            {
                Id = customerLineDto.AgentId.GetValueOrDefault(0),
                Name = customerLineDto.AgentName,
                Currency = customerLineDto.Currency.GetValueOrDefault(0),
                Level = Level.Agent
            };

            return agent;
        }

        private Customer GetCustomer(CustomerLineDto customerLineDto)
        {
            var customer = new Customer
            {
                Id = customerLineDto.CustId.GetValueOrDefault(0),
                Name = customerLineDto.UserName,
                Currency = customerLineDto.Currency.GetValueOrDefault(0),
                Level = Enumeration.FromValue<Level>((byte)customerLineDto.LevelValue)
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