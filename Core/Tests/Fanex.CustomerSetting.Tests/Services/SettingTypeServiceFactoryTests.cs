namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using CustomerSetting.Services.Impl;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeServiceFactoryTests
    {
        [Test]
        public void GetInstance_SettingTypeIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            var factory = new SettingTypeServiceFactory();

            //// Act
            Action action = () => factory.GetInstance(null);

            //// Assert
            action.ShouldThrow<ArgumentNullException>("settingType");
        }
    }
}