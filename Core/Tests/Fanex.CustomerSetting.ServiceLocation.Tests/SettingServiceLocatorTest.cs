namespace Fanex.CustomerSetting.ServiceLocation.Tests
{
    using System;
    using Casino.Services.Impl;
    using ServiceLocation;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using Services.Impl;
    using static Products;


    [TestFixture]
    public class SettingServiceLocatorTest
    {
        private SettingServiceConfigurator configurator;
        private static readonly Type QueryServiceDefaultType = typeof(QuerySettingService);
        private static readonly Type UpdateServiceDefaultType = typeof(UpdateSettingService);

        [SetUp]
        public void SetUpServiceTypeConfig()
        {
            configurator = new SettingServiceConfigurator();            
        }

        [Test]
        public void GetQueryService_SystemIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceFactory().GetQueryService(
                    Arg.Is<byte>(0),
                    Arg.Any<byte>(),
                    Arg.Any<ISettingContext>()));
        }

        [Test]
        public void GetUpdateService_SystemIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceFactory().GetUpdateService(
                    Arg.Is<byte>(0),
                    Arg.Any<byte>(),
                    Arg.Any<ISettingContext>()));
        }

        [Test]
        public void GetQueryService_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceFactory().GetQueryService(1, 0, Arg.Any<ISettingContext>()));



            //new SettingServiceFactory()
            //    .GetQueryService(1, 0, Arg.Any<ISettingContext>())
            //    .ReturnsForAnyArgs(x => { throw new ArgumentOutOfRangeException(); });
        }

        [Test]
        public void GetUpdateService_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceFactory().GetUpdateService(1, 0, Arg.Any<ISettingContext>()));
        }

        [TestCase(1, 100)]
        [TestCase(2, 101)]
        public void GetQueryService_ConfigurationIsEmpty_ReturnDefaultQueryService(byte system, byte productId)
        {
            //// Arrange            
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();

            var defaultServiceType = QueryServiceDefaultType;

            //// Act
            var settingService = new SettingServiceFactory()
                .GetQueryService(system, productId, settingContext);

            //// Assert
            Assert.AreEqual(settingService.GetType(), defaultServiceType, productId.ToString(), null);
        }

        [TestCase(1, 100)]
        [TestCase(2, 101)]
        public void GetUpdateService_ConfigurationIsEmpty_ReturnDefaultUpdateService(byte system, byte productId)
        {
            //// Arrange            
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();

            var defaultServiceType = UpdateServiceDefaultType;

            //// Act
            var settingService = new SettingServiceFactory()
                .GetUpdateService(system, productId, settingContext);

            //// Assert
            Assert.AreEqual(settingService.GetType(), defaultServiceType, productId.ToString(), null);
        }

        [TestCase(1, 100)]
        [TestCase(2, 101)]
        public void GetQueryService_ProductIdIsNotRegistered_ReturnDefaultQueryService(byte system, byte productId)
        {
            //// Arrange            
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();
            var defaultServiceType = QueryServiceDefaultType;

            //// Act
            var settingService = new SettingServiceFactory()
                .GetQueryService(system, productId, settingContext);

            //// Assert
            Assert.AreEqual(settingService.GetType(), defaultServiceType, productId.ToString(), null);
        }

        [TestCase(1, 100)]
        [TestCase(2, 101)]
        public void GetUpdateService_ProductIdIsNotRegistered_ReturnDefaultUpdateService(byte system, byte productId)
        {
            //// Arrange            
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();
            var defaultServiceType = UpdateServiceDefaultType;

            //// Act
            var settingService = new SettingServiceFactory()
                .GetUpdateService(system, productId, settingContext);

            //// Assert
            Assert.AreEqual(settingService.GetType(), defaultServiceType, productId.ToString(), null);
        }

        [Test]
        public void GetUpdateService_ProductIdIsRegistered_ReturnApproriatedValidService()
        {
            //// Arrange
            var productId = AllBet;
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();           
            var serviceType = typeof(UpdateCasinoSettingService);

            configurator
               .UseQueryServiceType(serviceType)
               .UseUpdateServiceType(serviceType)
               .ForProducts(AllBet)
               .Apply();

            //// Act
            var settingService = new SettingServiceFactory().GetUpdateService(100, productId, settingContext);

            //// Assert
            Assert.AreEqual(settingService.GetType(), serviceType, productId.ToString(), null);
        }

        [Test]
        public void GetQueryService_ProductIdIsRegistered_ReturnApproriatedValidService()
        {
            //// Arrange
            var productId = AllBet;
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();
            var serviceType = typeof(QueryCasinoSettingService);

            configurator
                .UseQueryServiceType(serviceType)
                .UseUpdateServiceType(serviceType)
                .ForProducts(AllBet)
                .Apply();

            //// Act
            var settingService = new SettingServiceFactory().GetQueryService(100, productId, settingContext);

            //// Assert
            Assert.AreEqual(settingService.GetType(), serviceType, productId.ToString(), null);
        }
    }
}
