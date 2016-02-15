namespace Fanex.CustomerSetting.ServiceLocation.Tests
{
    using System;
    using Casino.Services.Impl;
    using Fanex.CustomerSetting.ServiceLocation;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using Services.Impl;
    using static Products;
    using static SettingServiceBootstraper;

    [TestFixture]
    public class SettingServiceLocatorTest
    {
        private static readonly Type QueryServiceDefaultType = typeof(QuerySettingService);
        private static readonly Type UpdateServiceDefaultType = typeof(UpdateSettingService);

        private void SetUpServiceTypeConfig(params byte[] productIds)
        {
            Clear();

            foreach (var productId in productIds)
            {
                var config = new SettingServiceConfiguration
                {
                    ProductId = productId,
                    QueryServiceType = typeof(QuerySettingService),
                    UpdateServiceType = typeof(UpdateSettingService)
                };

                RegisterService(config);
            }
        }

        [Test]
        public void GetQueryService_SystemIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceLocator().GetQueryService(
                    Arg.Is<byte>(0),
                    Arg.Any<byte>(),
                    Arg.Any<ISettingContext>()));
        }

        [Test]
        public void GetUpdateService_SystemIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceLocator().GetUpdateService(
                    Arg.Is<byte>(0),
                    Arg.Any<byte>(),
                    Arg.Any<ISettingContext>()));
        }

        [Test]
        public void GetQueryService_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceLocator().GetQueryService(1, 0, Arg.Any<ISettingContext>()));
        }

        [Test]
        public void GetUpdateService_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceLocator().GetUpdateService(1, 0, Arg.Any<ISettingContext>()));
        }

        [TestCase(1, 100)]
        [TestCase(2, 101)]
        public void GetQueryService_ConfigurationIsEmpty_ReturnDefaultQueryService(byte system, byte productId)
        {
            //// Arrange            
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();
            Clear();
            var defaultServiceType = QueryServiceDefaultType;

            //// Act
            var settingService = new SettingServiceLocator()
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
            Clear();
            var defaultServiceType = UpdateServiceDefaultType;

            //// Act
            var settingService = new SettingServiceLocator()
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
            SetUpServiceTypeConfig(AllBet, Sportsbook2);
            var defaultServiceType = QueryServiceDefaultType;

            //// Act
            var settingService = new SettingServiceLocator()
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
            SetUpServiceTypeConfig(AllBet, Sportsbook2);
            var defaultServiceType = UpdateServiceDefaultType;

            //// Act
            var settingService = new SettingServiceLocator()
                .GetUpdateService(system, productId, settingContext);

            //// Assert
            Assert.AreEqual(settingService.GetType(), defaultServiceType, productId.ToString(), null);
        }

        [Test]
        public void GetQueryService_ProductIdIsRegistered_ReturnApproriatedValidService()
        {
            //// Arrange
            var productId = AllBet;
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();
            SetUpServiceTypeConfig(Sportsbook2, Cricket);
            var serviceType = typeof(QueryCasinoSettingService);

            var casinoConfig = new SettingServiceConfiguration
            {
                ProductId = productId,
                QueryServiceType = serviceType,
                UpdateServiceType = serviceType
            };

            RegisterService(casinoConfig);

            //// Act
            var settingService = new SettingServiceLocator().GetQueryService(100, productId, settingContext);

            //// Assert
            Assert.AreEqual(settingService.GetType(), serviceType, productId.ToString(), null);
        }

        [Test]
        public void GetUpdateService_ProductIdIsRegistered_ReturnApproriatedValidService()
        {
            //// Arrange
            var productId = AllBet;
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();
            SetUpServiceTypeConfig(Sportsbook2, Cricket);
            var serviceType = typeof(UpdateCasinoSettingService);

            var casinoConfig = new SettingServiceConfiguration
            {
                ProductId = productId,
                QueryServiceType = serviceType,
                UpdateServiceType = serviceType
            };

            RegisterService(casinoConfig);

            //// Act
            var settingService = new SettingServiceLocator().GetUpdateService(100, productId, settingContext);

            //// Assert
            Assert.AreEqual(settingService.GetType(), serviceType, productId.ToString(), null);
        }
    }
}
