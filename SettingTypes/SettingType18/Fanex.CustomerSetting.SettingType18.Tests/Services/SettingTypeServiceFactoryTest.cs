namespace Fanex.CustomerSetting.SettingType18.Tests.Services
{
    using CustomerSetting.LoggingConfig;
    using CustomerSetting.Models;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using NUnit.Framework;
    using SettingType18.Services;
    using Fanex.Logging.Extensions.Database;

    [TestFixture]
    public class SettingTypeServiceFactoryTest
    {
        [SetUp]
        public void Setup()
        {
            LoggingConfig.Init(new[] { "pass", "pwd", "security", "Password" }, getDbExceptionInfo: DatabaseInfoGetters.GetDbExceptionInfo);
        }

        [Test]
        public void GetInstance_SettingTypeIsPT_ReturnValidSettingTypeService()
        {
            //// Arrange
            var factory = new SettingTypeServiceFactory();
            var settingType = new SettingType { Id = 18 };
            ISettingTypeService expected = new SettingTypeService();

            //// Act
            ISettingTypeService actual = factory.GetInstance(settingType);

            //// Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
    }
}