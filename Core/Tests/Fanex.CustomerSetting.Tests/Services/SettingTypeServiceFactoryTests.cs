namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using CustomerSetting.Services.Impl;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeServiceFactoryTests
    {
        [Test]
        public void GetInstance_SettingTypeIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            var factory = new SettingTypeServiceFactory();

            //// Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => factory.GetInstance(null));
            Assert.That(ex.ParamName, Is.EqualTo("settingType"));
        }
    }
}