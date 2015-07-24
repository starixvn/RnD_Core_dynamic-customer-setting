namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Services
{
    using System.Linq;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType1.Template1.Resources;
    using Fanex.CustomerSetting.SettingType1.Template1.Services;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.CustomerSetting.Tests.Services;
    using NSubstitute;
    using NUnit.Framework;
    using FluentAssertions;
    using System;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Models;

    [TestFixture]
    public class SettingTypeServiceFactoryTest
    {
        [Test]
        public void GetInstance_SettingTypeIsPT_ReturnValidSettingTypeService()
        {
            //// Arrange
            var factory = new SettingTypeServiceFactory();
            var settingType = new SettingType { Id = 1, SettingTemplate = 1 };
            ISettingTypeService expected = new SettingTypeService();

            //// Act
            ISettingTypeService actual = factory.GetInstance(settingType);

            //// Assert
            actual.GetType().ShouldBeEquivalentTo<Type>(expected.GetType());
        }
    }
}
