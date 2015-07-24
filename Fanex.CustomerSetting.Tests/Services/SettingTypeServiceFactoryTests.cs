namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using Fanex.CustomerSetting.Services.Impl;
    using NUnit.Framework;
    using FluentAssertions;

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