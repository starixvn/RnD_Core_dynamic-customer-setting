namespace Fanex.CustomerSetting.Tests.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Results;
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingTypeTests
    {
        [Test]
        public void SettingItemsProperty_SetNullValueThenGet_ReturnEmptySettingItem()
        {
            //// Arrange
            var settingType = new SettingType();

            //// Act
            settingType.SettingItems = null;    
            
            //// Assert
            settingType.SettingItems.Should().BeEmpty();
        }

        [Test]
        public void TypeName_Always_ReturnExpectedValue()
        {
            var settingType = new SettingType();

            settingType.TypeName.ShouldBeEquivalentTo<string>(settingType.GetType().AssemblyQualifiedName);
        }

        [Test]
        public void ToXml_Always_ReturnFormattedXml()
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
            Assert.AreEqual("<ROOT>SettingItemStub</ROOT>", actualXmlFormatted);
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
            Assert.AreEqual(ResultValue.NoChange, actualResult.Code);
        }

        [Test]
        public void Validate_SettingItemIsNotValid_ReturnErrorResult()
        {
            //// Arrange
            var invalidSettingItem = Substitute.For<ISettingItem>();
            invalidSettingItem.Validate().Returns(Result.DataError);
            var validSettingItem = Substitute.For<ISettingItem>();
            validSettingItem.Validate().Returns(Result.Success);
            var settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                {
                    invalidSettingItem,
                    validSettingItem
                }
            };

            //// Act
            var actualResult = settingType.Validate();

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void Validate_AllSettingItemsAreValid_ReturnSuccessResult()
        {
            //// Arrange
            var validSettingItem = Substitute.For<ISettingItem>();
            validSettingItem.Validate().Returns(Result.Success);
            var settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                {
                    validSettingItem
                }
            };

            //// Act
            var actualResult = settingType.Validate();

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test] 
        public void ToStringMethod_Always_ReturnCombinationOfIdAndTemplate()
        {
            //// Arrange
            short settingTypeId = 1;
            byte template = 3;
            SettingType settingType = new SettingType
            {
                Id = settingTypeId,
                SettingTemplate = template
            };

            //// Act
            var actual = settingType.ToString();

            //// Assert
            Assert.AreEqual(string.Join("_", settingTypeId, template), actual);
        }
    }
}