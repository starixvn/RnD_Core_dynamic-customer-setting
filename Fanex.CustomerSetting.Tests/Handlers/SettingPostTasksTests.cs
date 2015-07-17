namespace Fanex.CustomerSetting.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Tests.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public abstract class SettingPostTasksTests
    {
        protected IResourceService ResourceService { get; set; }

        protected ISettingContext SettingContext { get; set; }

        protected SettingPostTasks SettingPostTasks { get; set; }

        public virtual void PostGetCustomerSettingType_SettingContextIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            SettingType settingType = new SettingType();
            SettingContext = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => SettingPostTasks.PostGetCustomerSettingType(
                SettingContext,
                null,
                settingType)));
        }

        public virtual void PostGetCustomerSettingType_WhenCalled_SetSportTypeBetTypeNameForSettingItems()
        {
            //// Arrange
            SettingType settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                    {
                        new DefaultSettingItem
                        {
                            BetTypeId = 1,
                            SportTypeId = 1
                        }
                    }
            };
            var sportTypeName = "SportTypeName";
            var betTypeName = "BetTypeName";
            ResourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            ResourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act

            var actualSettingType = SettingPostTasks.PostGetCustomerSettingType(
                SettingContext,
                null,
                settingType);

            //// Assert
            DefaultSettingItem settingItem = actualSettingType.SettingItems.First() as DefaultSettingItem;
            Assert.AreEqual(sportTypeName, settingItem.SportTypeName);
            Assert.AreEqual(betTypeName, settingItem.BetTypeName);
        }

        public virtual void PostGetCustomerSettingType_TypeOfSettingItemIsNotDefaultSettingItem_ThrowInvalidCastException()
        {
            //// Arrange
            SettingType settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                    {
                        new SettingItemStub()
                    }
            };

            //// Act
            Assert.Throws<InvalidCastException>(
                new TestDelegate(
                    () => SettingPostTasks.PostGetCustomerSettingType(
                SettingContext,
                null,
                settingType)));
        }

        public virtual void PostUpdateCustomerSettingType_SettingContextIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            SettingType settingType = new SettingType();
            SettingContext = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => SettingPostTasks.PostUpdateCustomerSettingType(SettingContext, 1, UserLevel.Agent, settingType, null, null)));
        }

        public virtual void PostUpdateCustomerSettingType_SettingTypeIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            SettingType settingType = null;
            SettingContext = new SettingContextStub();

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => SettingPostTasks.PostUpdateCustomerSettingType(SettingContext, 1, UserLevel.Agent, settingType, null, null)));
        }
    }
}