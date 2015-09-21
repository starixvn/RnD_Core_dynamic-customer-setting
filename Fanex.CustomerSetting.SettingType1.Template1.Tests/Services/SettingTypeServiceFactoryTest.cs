namespace Fanex.CustomerSetting.SettingType1.Tests.Services
{
    using System;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType1.Services;
    using FluentAssertions;
    using NUnit.Framework;

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
            actual.GetType().ShouldBeEquivalentTo<Type>(expected.GetType());
        }
    }
}
