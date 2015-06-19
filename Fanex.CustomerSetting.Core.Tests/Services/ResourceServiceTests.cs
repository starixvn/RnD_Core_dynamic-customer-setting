namespace Fanex.CustomerSetting.Core.Tests.Services
{
    using System;
    using Fanex.CustomerSetting.Core.Handlers;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Services;
    using Fanex.CustomerSetting.Core.Services.Impl;
    using Fanex.CustomerSetting.Core.Tests.Models;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ResourceServiceTests
    {
        private IResourceService _resourceService;
        private IDynamicRepository _dymanicRepository;
        private ISettingContext _settingContext;
        private IRemotingRefApi _remotingRefApi;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _dymanicRepository = Substitute.For<IDynamicRepository>();
            _remotingRefApi = Substitute.For<IRemotingRefApi>();
            RemotingRefApiHandler.RemotingRefApi = _remotingRefApi;
            _resourceService = new ResourceService(_settingContext, _dymanicRepository);
        }

        #region Get Sport Name

        [TestCase(0)]
        [TestCase(-1)]
        public void GetSportTypeName_SportTypeIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int sportTypeId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _resourceService.GetSportTypeName(sportTypeId)));
        }

        [Test]
        public void GetSportTypeName_ExecuteGetSportTypeNameOnRemotingRefApi()
        {
            //// Arrange
            int sportTypeId = 1;

            //// Act
            _resourceService.GetSportTypeName(sportTypeId);

            //// Assert
            _remotingRefApi.Received().GetSportTypeName(
                Arg.Is<string>(
                    sportTypeIdString => sportTypeIdString == sportTypeId.ToString()), 
                Arg.Any<string>());
        }

        #endregion Get Sport Name

        #region Get Bet Type Name

        [TestCase(0)]
        [TestCase(-1)]
        public void GetBetTypeName_BetTypeIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int betTypeId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _resourceService.GetBetTypeName(betTypeId)));
        }

        [Test]
        public void GetBetTypeName_ExecuteGetSportTypeNameOnRemotingRefApi()
        {
            //// Arrange
            int betTypeId = 1;

            //// Act
            _resourceService.GetBetTypeName(betTypeId);

            //// Assert
            _remotingRefApi.Received().GetBetTypeName(
                Arg.Is<string>(
                    betTypeIdString => betTypeIdString == betTypeId.ToString()), 
                Arg.Any<string>());
        }

        #endregion Get Bet Type Name
    }
}