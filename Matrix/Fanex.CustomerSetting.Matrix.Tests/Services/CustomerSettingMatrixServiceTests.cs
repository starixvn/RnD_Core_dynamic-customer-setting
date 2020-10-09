namespace Fanex.CustomerSetting.Matrix.Tests.Services
{
    using Fanex.CustomerSetting.Matrix.Services;
    using Fanex.CustomerSetting.Matrix.Services.Impl;
    using Fanex.CustomerSetting.Services;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerSettingMatrixServiceTests
    {
        private ICustomerService _customerService;
        private IPTMatrixService _pTMatrixService;
        private ICommissionMatrixService _commissionMatrixService;
        private IResourceService _resourceService;
        private ICachingService _cachingService;
        private IDynamicRepository _dynamicRepository;

        private ICustomerSettingMatrixService _customerSettingMatrixService;

        [SetUp]
        public void SetUp()
        {
            _customerService = Substitute.For<ICustomerService>();
            _pTMatrixService = Substitute.For<IPTMatrixService>();
            _commissionMatrixService = Substitute.For<ICommissionMatrixService>();
            _resourceService = Substitute.For<IResourceService>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _cachingService = Substitute.For<ICachingService>();

            _customerSettingMatrixService = new CustomerSettingMatrixService(
                _customerService,
                _pTMatrixService,
                _commissionMatrixService,
                _resourceService,
                _cachingService,
                _dynamicRepository);
        }

        [Test]
        public void GetCustomerSettingMatrix_ValidQuery_Success()
        {

        }
    }
}
