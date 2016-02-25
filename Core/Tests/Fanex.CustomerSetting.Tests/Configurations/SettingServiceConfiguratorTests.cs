
namespace Fanex.CustomerSetting.Tests.Configurations
{
    using System;
    using CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Configurations;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    [Category("SettingServiceConfigurator")]
    public class SettingServiceConfiguratorTests
    {
        private static readonly Type DefaultQueryServiceType = typeof(QuerySettingService);
        private static readonly Type DefaultUpdateServiceType = typeof(UpdateSettingService);

        private static readonly object[] ServiceTypeCases =
        {
            new object[] { true, DefaultQueryServiceType },
            new object[] { false, DefaultUpdateServiceType }
        };


        private SettingServiceConfigurator configurator;

        [SetUp]
        public void Setup()
        {
            configurator = new SettingServiceConfigurator();
        }

        [Test]
        public void GetServiceType_SystemIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            byte system = 0;

            //// Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                    () => SettingServiceConfigurator.GetServiceType(system, 1, true));
        }

        [Test]
        public void GetServiceType_ProductIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            byte system = 1;
            byte product = 0;

            //// Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                    () => SettingServiceConfigurator.GetServiceType(system, product, true));
        }

        [Test, TestCaseSource("ServiceTypeCases")]
        public void GetServiceType_ConfigIsNotExist_ReturnDefaultServiceType(bool isQueryAction, Type serviceType)
        {
            //// Arrange
            byte system = 1;
            byte product = 1;
            configurator.Clear();

            //// Act
            var actualServiceType = SettingServiceConfigurator.GetServiceType(system, product, isQueryAction);

            //// Assert
            Assert.AreEqual(serviceType, actualServiceType);
        }

        [Test, TestCaseSource("ServiceTypeCases")]
        public void GetServiceType_ConfigDoesNotSetForASpecifiedSystem_ReturnApproriatedTypeGetFromThatConfig(bool isQueryAction, Type serviceType)
        {
            //// Arrange
            byte system = 1;
            byte product = 1;
            configurator
                .UseQueryServiceType(DefaultQueryServiceType)
                .UseUpdateServiceType(DefaultUpdateServiceType)
                .ForProducts(1)
                .Apply();

            //// Act
            var actualServiceType = SettingServiceConfigurator.GetServiceType(system, product, isQueryAction);

            //// Assert
            Assert.AreEqual(serviceType, actualServiceType);

        }
    }
}
