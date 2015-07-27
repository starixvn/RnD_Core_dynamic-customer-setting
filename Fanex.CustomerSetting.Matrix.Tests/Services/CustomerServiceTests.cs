namespace Fanex.CustomerSetting.Matrix.Tests.Services
{
    using System.Linq;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Services;
    using Fanex.CustomerSetting.Matrix.Services.Impl;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerServiceTests
    {
        private IDynamicRepository _dynamicRepository;

        private ICustomerService _customerService;

        [SetUp]
        public void SetUp()
        {
            _dynamicRepository = Substitute.For<IDynamicRepository>();

            _customerService = new CustomerService(_dynamicRepository);
        }

        [Test]
        public void GetCustomerLine_Always_CallGetCustomerLineDto()
        {
            //// Arrange
            var customerName = "customerName";
            var customerLineDto = CreateCustomerLineDto();
            _dynamicRepository.Get<CustomerLineDto>(Arg.Any<CustomerLineQueryCriteria>()).Returns(customerLineDto);

            //// Act
            _customerService.GetCustomerLine(customerName);

            //// Assert
            _dynamicRepository.Received().Get<CustomerLineDto>(Arg.Is<CustomerLineQueryCriteria>(
                criteria => criteria.UserName == customerName));
        }

        [Test]
        public void GetCustomerLine_CustomerIsSuper_ReturnSuperLine()
        {
            //// Arrange
            var customerName = "customerName";
            var customerLineDto = CreateCustomerLineDto(Level.Super);
            _dynamicRepository.Get<CustomerLineDto>(Arg.Any<CustomerLineQueryCriteria>()).Returns(customerLineDto);

            //// Act
            var actualCustomerLine = _customerService.GetCustomerLine(customerName);

            //// Assert
            Assert.AreEqual(1, actualCustomerLine.Count());
            Assert.AreEqual(customerLineDto.CustId, actualCustomerLine.First().Id);
            Assert.AreEqual(customerLineDto.UserName, actualCustomerLine.First().Name);
        }

        [Test]
        public void GetCustomerLine_CustomerIsMaster_ReturnMasterLine()
        {
            //// Arrange
            var customerName = "customerName";
            var customerLineDto = CreateCustomerLineDto(Level.Master);
            _dynamicRepository.Get<CustomerLineDto>(Arg.Any<CustomerLineQueryCriteria>()).Returns(customerLineDto);

            //// Act
            var actualCustomerLine = _customerService.GetCustomerLine(customerName);

            //// Assert
            Assert.AreEqual(2, actualCustomerLine.Count());
            Assert.AreEqual(customerLineDto.SuperId, actualCustomerLine.ElementAt(0).Id);
            Assert.AreEqual(customerLineDto.SuperName, actualCustomerLine.ElementAt(0).Name);
            Assert.AreEqual(customerLineDto.CustId, actualCustomerLine.ElementAt(1).Id);
            Assert.AreEqual(customerLineDto.UserName, actualCustomerLine.ElementAt(1).Name);
        }

        [Test]
        public void GetCustomerLine_CustomerIsAgent_ReturnAgentLine()
        {
            //// Arrange
            var customerName = "customerName";
            var customerLineDto = CreateCustomerLineDto(Level.Agent);
            _dynamicRepository.Get<CustomerLineDto>(Arg.Any<CustomerLineQueryCriteria>()).Returns(customerLineDto);

            //// Act
            var actualCustomerLine = _customerService.GetCustomerLine(customerName);

            //// Assert
            Assert.AreEqual(3, actualCustomerLine.Count());
            Assert.AreEqual(customerLineDto.SuperId, actualCustomerLine.ElementAt(0).Id);
            Assert.AreEqual(customerLineDto.SuperName, actualCustomerLine.ElementAt(0).Name);
            Assert.AreEqual(customerLineDto.MasterId, actualCustomerLine.ElementAt(1).Id);
            Assert.AreEqual(customerLineDto.MasterName, actualCustomerLine.ElementAt(1).Name);
            Assert.AreEqual(customerLineDto.CustId, actualCustomerLine.ElementAt(2).Id);
            Assert.AreEqual(customerLineDto.UserName, actualCustomerLine.ElementAt(2).Name);
        }

        [Test]
        public void GetCustomerLine_CustomerIsAgent_ReturnMemberLine()
        {
            //// Arrange
            var customerName = "customerName";
            var customerLineDto = CreateCustomerLineDto(Level.Member);
            _dynamicRepository.Get<CustomerLineDto>(Arg.Any<CustomerLineQueryCriteria>()).Returns(customerLineDto);

            //// Act
            var actualCustomerLine = _customerService.GetCustomerLine(customerName);

            //// Assert
            Assert.AreEqual(4, actualCustomerLine.Count());
            Assert.AreEqual(customerLineDto.SuperId, actualCustomerLine.ElementAt(0).Id);
            Assert.AreEqual(customerLineDto.SuperName, actualCustomerLine.ElementAt(0).Name);
            Assert.AreEqual(customerLineDto.MasterId, actualCustomerLine.ElementAt(1).Id);
            Assert.AreEqual(customerLineDto.MasterName, actualCustomerLine.ElementAt(1).Name);
            Assert.AreEqual(customerLineDto.AgentId, actualCustomerLine.ElementAt(2).Id);
            Assert.AreEqual(customerLineDto.AgentName, actualCustomerLine.ElementAt(2).Name);
            Assert.AreEqual(customerLineDto.CustId, actualCustomerLine.ElementAt(3).Id);
            Assert.AreEqual(customerLineDto.UserName, actualCustomerLine.ElementAt(3).Name);
        }

        private CustomerLineDto CreateCustomerLineDto(Level customerLevel = null)
        {
            customerLevel = customerLevel ?? Level.Member;

            return new CustomerLineDto
            {
                CustId = 1,
                LevelValue = customerLevel.Value,
                SuperId = 2,
                SuperName = "SuperName",
                MasterId = 3,
                MasterName = "MasterName",
                AgentId = 4,
                AgentName = "AgentName",
                Currency = 1
            };
        }
    }
}