namespace Fanex.CustomerSetting.SettingType4.Template1.Tests.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Dtos.Messages;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.SettingType4.Template1.Handlers;
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using Fanex.CustomerSetting.Tests.Models;
    using NSubstitute;
    using NUnit.Framework;

    public class SettingType4PostTasksTests
    {
        private ISettingContext _settingContext;
        private IMessageService _messageService;
        private IResourceService _resourceService;
        private SettingType4PostTasks _settingType3PostTasks;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _resourceService = Substitute.For<IResourceService>();
            _messageService = Substitute.For<IMessageService>();
            _settingType3PostTasks = new SettingType4PostTasks(_messageService, _resourceService);
        }

        #region Post Get Customer Setting Type

        [Test]
        public void PostGetCustomerSettingType_SetSportTypeBetTypeNameForSettingItems()
        {
            //// Arrange
            SettingType settingType = new SettingType
            {
                SettingItems = new List<ISettingItem>
                    {
                        new ProductStatusItem
                        {
                            BetTypeId = 1,
                            SportTypeId = 1
                        }
                    }
            };
            var sportTypeName = "SportTypeName";
            var betTypeName = "BetTypeName";
            _resourceService.GetSportTypeName(1, Arg.Any<string>()).Returns(sportTypeName);
            _resourceService.GetBetTypeName(1, Arg.Any<string>()).Returns(betTypeName);

            //// Act

            var actualSettingType = _settingType3PostTasks.PostGetCustomerSettingType(
                _settingContext,
                null,
                settingType);

            //// Assert
            ProductStatusItem settingItem = actualSettingType.SettingItems.First() as ProductStatusItem;
            Assert.AreEqual(sportTypeName, settingItem.SportTypeName);
            Assert.AreEqual(betTypeName, settingItem.BetTypeName);
        }

        #endregion Post Get Customer Setting Type

        #region Post Update Customer Setting Type

        [Test]
        public void PostUpdateCustomerSettingType_ResultIsSuccessful_ReturnUpdatePostionTakingSuccessfulResult()
        {
            //// Arrange
            var successResult = Result.Success;

            //// Act
            var actualResult = _settingType3PostTasks.PostUpdateCustomerSettingType(
                _settingContext,
                null,
                new SettingType { SettingLevel = 1 },
                null,
                successResult);

            //// Assert
            Assert.AreEqual(Lang.UpdateProductStatusSuccessfully, actualResult.Message);
        }

        [Test]
        public void PostUpdateCustomerSettingType_ResultIsFailed_ExecuteTranslateMessageInMessageService()
        {
            //// Arrange
            var successResult = Result.DataError;
            _messageService.TranslateMessage(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>())
                .Returns(new MessageContent(string.Empty));
            byte settingLevel = 1;

            //// Act
            _settingType3PostTasks.PostUpdateCustomerSettingType(
                _settingContext,
                null,
                new SettingType
                {
                    SettingLevel = settingLevel
                },
                null,
                successResult);

            //// Assert
            _messageService.Received().TranslateMessage(successResult.MsgType, successResult.Message, settingLevel, _settingContext.Language);
        }

        #endregion Post Update Customer Setting Type
    }
}