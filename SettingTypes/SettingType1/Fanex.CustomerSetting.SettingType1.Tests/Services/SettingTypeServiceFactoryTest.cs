namespace Fanex.CustomerSetting.SettingType1.Tests.Services
{
    using System;
    using CustomerSetting.Models;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using FluentAssertions;
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
            actual.GetType().ShouldBeEquivalentTo<Type>(expected.GetType());
        }
    }
}