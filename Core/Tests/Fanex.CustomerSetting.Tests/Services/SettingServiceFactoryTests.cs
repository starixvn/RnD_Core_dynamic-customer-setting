namespace Fanex.CustomerSetting.Tests.Tests
{
    using System;
    using Casino.Services.Impl;
    using CustomerSetting.Configurations;
    using CustomerSetting.Models;
    using CustomerSetting.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;


    [TestFixture]
    public class SettingServiceFactoryTests
    {
        private const byte AllBet = 21;
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
                () => new SettingServiceFactory().CreateQueryService(
                    Arg.Is<byte>(0),
                    Arg.Any<byte>(),
                    Arg.Any<ISettingContext>()));
        }

        [Test]
        public void GetUpdateService_SystemIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceFactory().CreateUpdateService(
                    Arg.Is<byte>(0),
                    Arg.Any<byte>(),
                    Arg.Any<ISettingContext>()));
        }

        [Test]
        public void GetQueryService_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceFactory().CreateQueryService(1, 0, Arg.Any<ISettingContext>()));
        }

        [Test]
        public void GetUpdateService_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceFactory().CreateUpdateService(1, 0, Arg.Any<ISettingContext>()));
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
                .CreateQueryService(system, productId, settingContext);

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
                .CreateUpdateService(system, productId, settingContext);

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
                .CreateQueryService(system, productId, settingContext);

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
                .CreateUpdateService(system, productId, settingContext);

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
            var settingService = new SettingServiceFactory().CreateUpdateService(100, productId, settingContext);

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
            var settingService = new SettingServiceFactory().CreateQueryService(100, productId, settingContext);

            //// Assert
            Assert.AreEqual(settingService.GetType(), serviceType, productId.ToString(), null);
        }
    }
}
