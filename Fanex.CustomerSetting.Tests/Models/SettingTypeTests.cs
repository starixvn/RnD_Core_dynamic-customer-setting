namespace Fanex.CustomerSetting.Tests.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeTests
    {
        [Test]
        public void ToXml_ReturnFormattedXml()
        {
            //// Arrange
            var settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                {
                    new SettingItemStub()
                }
            };

            //// Act
            var actualXmlFormatted = settingType.ToXml();

            //// Assert
            Assert.AreEqual("<ROOT></ROOT>", actualXmlFormatted);
        }

        [Test]
        public void Validate_SettingTypeHasNoSettingItem_ReturnNoChangeResult()
        {
            //// Arrange
            var settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>()
            };

            //// Act
            var actualResult = settingType.Validate();

            //// Assert
            Assert.AreEqual(Result.NoChangeValue, actualResult.Code);
        }

        [Test]
        public void Validate_SettingTypeHasSettingItemsViolationRule_ReturnErrorResult()
        {
            //// Arrange
            var violationSettingItem = Substitute.For<ISettingItem>();
            violationSettingItem.Validate().Returns(Result.DataError);
            var noviolationItem = Substitute.For<ISettingItem>();
            noviolationItem.Validate().Returns(Result.Success);
            var settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                {
                    violationSettingItem,
                    noviolationItem
                }
            };

            //// Act
            var actualResult = settingType.Validate();

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void Validate_SettingTypeHasNoSettingItemsViolationRule_ReturnSuccessResult()
        {
            //// Arrange
            var noviolationItem = Substitute.For<ISettingItem>();
            noviolationItem.Validate().Returns(Result.Success);
            var settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                {
                    noviolationItem
                }
            };

            //// Act
            var actualResult = settingType.Validate();

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }
    }
}