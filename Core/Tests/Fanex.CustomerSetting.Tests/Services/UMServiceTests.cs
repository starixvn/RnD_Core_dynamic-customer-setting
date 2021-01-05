namespace Fanex.CustomerSetting.Tests.Services
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class UMServiceTests
    {
        private IUMService _UMService;
        private IDynamicRepository _dynamicRepository;
        private IResourceService _resourceService;

        [SetUp]
        public void SetUp()
        {

            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _resourceService = Substitute.For<IResourceService>();
            _UMService = new UMService(_dynamicRepository, _resourceService);
        }

        [Test]
        public void UMService_ParameterlessConstructor_Success()
        {
            var UMService = new UMService();
            Assert.That(UMService != null);
        }


        [Test]
        public void GetUMResult_NoData_ReturnNull()
        {
            Assert.AreEqual(null, _UMService.GetUMResult(0, 0));
        }

        [Test]
        public void GetUMResult_HaveData_ReturnExpectedResult()
        {
            var beginUMTime = new DateTime(2020, 1, 1, 0, 0, 0);
            var endUMTime = new DateTime(2020, 1, 1, 1, 0, 0);
            var umInfos = new List<UnderMaintainanceInfo>
            {
                new UnderMaintainanceInfo(0,0,0,beginUMTime, endUMTime),
                new UnderMaintainanceInfo(22,212,0,beginUMTime, endUMTime),
                new UnderMaintainanceInfo(22,232,0,beginUMTime, endUMTime),
            };

            _dynamicRepository.Fetch<UnderMaintainanceInfo>(Arg.Any<ICriteria>()).Returns(umInfos);

            var actual = _UMService.GetUMResult(0, 0);
            Assert.That(actual.SubMessages.ToList().Count == 3);
        }
    }
}
