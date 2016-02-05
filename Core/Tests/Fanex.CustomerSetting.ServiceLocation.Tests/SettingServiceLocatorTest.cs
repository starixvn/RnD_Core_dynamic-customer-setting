namespace Fanex.CustomerSetting.ServiceLocation.Tests
{
    using System;
    using Casino.Services;
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
        public void GetService_SystemIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceLocator().GetService(
                    Arg.Is<byte>(0),
                    Arg.Any<byte>(),
                    Arg.Any<bool>(),
                    Arg.Any<ISettingContext>()));
        }

        public void GetService_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Act & Asssert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SettingServiceLocator().GetService(1, 0, Arg.Any<bool>(), Arg.Any<ISettingContext>()));
        }

        [TestCase(1, 100, true)]
        [TestCase(2, 101, false)]
        public void GetService_ConfigurationIsEmpty_ReturnDefaultQueryService(byte system, byte productId, bool isQueryAction)
        {
            //// Arrange            
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();
            Clear();
            var defaultServiceType = (isQueryAction) ? QueryServiceDefaultType : UpdateServiceDefaultType;

            //// Act
            var querySettingService = new SettingServiceLocator()
                .GetService(system, productId, isQueryAction, settingContext);

            //// Assert
            Assert.AreEqual(querySettingService.GetType(), defaultServiceType, productId.ToString(), null);
        }

        [TestCase(1, 100, true)]
        [TestCase(2, 101, false)]
        public void GetService_ProductIdIsNotRegistered_ReturnDefaultQueryService(byte system, byte productId, bool isQueryAction)
        {
            //// Arrange            
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();
            SetUpServiceTypeConfig(AllBet, Sportsbook2);
            var defaultServiceType = (isQueryAction) ? QueryServiceDefaultType : UpdateServiceDefaultType;

            //// Act
            var querySettingService = new SettingServiceLocator()
                .GetService(system, productId, isQueryAction, settingContext);

            //// Assert
            Assert.AreEqual(querySettingService.GetType(), defaultServiceType, productId.ToString(), null);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetService_ProductIdIsRegistered_ReturnApproriatedValidService(bool isQueryAction)
        {
            //// Arrange
            var productId = AllBet;
            var settingContext = Substitute.For<ISettingContext>();
            settingContext.Actor = Substitute.For<User>();
            SetUpServiceTypeConfig(Sportsbook2, Cricket);
            var serviceType = (isQueryAction) ? typeof(QueryCasinoSettingService) : typeof(UpdateCasinoSettingService);

            var casinoConfig = new SettingServiceConfiguration
            {
                ProductId = productId,
                QueryServiceType = serviceType,
                UpdateServiceType = serviceType
            };

            SettingServiceBootstraper.RegisterService(casinoConfig);

            //// Act
            var querySettingService = new SettingServiceLocator().GetService(100, productId, isQueryAction, settingContext);

            //// Assert
            Assert.AreEqual(querySettingService.GetType(), serviceType, productId.ToString(), null);
        }
    }
}
