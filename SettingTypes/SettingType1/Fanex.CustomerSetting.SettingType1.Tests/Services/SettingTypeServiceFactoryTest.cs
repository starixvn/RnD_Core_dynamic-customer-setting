namespace Fanex.CustomerSetting.SettingType1.Tests.Services
{
    using CustomerSetting.Models;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using NUnit.Framework;
    using SettingType1.Services;

    [TestFixture]
    public class SettingTypeServiceFactoryTest
    {
        [Test]
        public void GetInstance_SettingTypeIsPT_ReturnValidSettingTypeService()
        {
            //// Arrange
            var factory = new SettingTypeServiceFactory();
            var settingType = new SettingType { Id = 1 };
            ISettingTypeService expected = new SettingTypeService();

            //// Act
            ISettingTypeService actual = factory.GetInstance(settingType);

            //// Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }
    }
}