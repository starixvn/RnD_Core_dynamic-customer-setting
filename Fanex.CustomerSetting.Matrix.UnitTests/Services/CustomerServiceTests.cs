namespace Fanex.CustomerSetting.Matrix.UnitTests.Services
{
    using Fanex.CustomerSetting.Matrix.Services;
    using Fanex.CustomerSetting.Matrix.Services.Impl;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerServiceTests
    {
        private IDynamicRepository _dynamicRepository;

        private ICustomerService _customerService;

        public void SetUp()
        {
            _dynamicRepository = Substitute.For<IDynamicRepository>();

            _customerService = new CustomerService(_dynamicRepository);
        }

        [Test]
        public void GetCustomerLine_Always_CallGetCustomerLineDto()
        {
        }
    }
}