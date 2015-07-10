namespace Fanex.CustomerSetting.SettingType3.Template1.Tests.Handlers
{
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType3.Template1.Handlers;
    using Fanex.CustomerSetting.Tests.Handlers;
    using Fanex.CustomerSetting.Tests.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingType3PostTasksTests : SettingPostTasksTests
    {
        private IMessageService _messageService;

        [SetUp]
        public void Setup()
        {
            SettingContext = new SettingContextStub();
            ResourceService = Substitute.For<IResourceService>();
            _messageService = Substitute.For<IMessageService>();
            SettingPostTasks = new SettingType3PostTasks(_messageService, ResourceService);
        }

        [Test]
        public override void PostGetCustomerSettingType_SettingContextIsNull_ThrowArgumentNullException()
        {
            base.PostGetCustomerSettingType_SettingContextIsNull_ThrowArgumentNullException();
        }

        [Test]
        public override void PostGetCustomerSettingType_TypeOfSettingItemIsNotDefaultSettingItem_ThrowInvalidCastException()
        {
            base.PostGetCustomerSettingType_TypeOfSettingItemIsNotDefaultSettingItem_ThrowInvalidCastException();
        }

        [Test]
        public override void PostGetCustomerSettingType_WhenCalled_SetSportTypeBetTypeNameForSettingItems()
        {
            base.PostGetCustomerSettingType_WhenCalled_SetSportTypeBetTypeNameForSettingItems();
        }

        [Test]
        public override void PostUpdateCustomerSettingType_SettingContextIsNull_ThrowArgumentNullException()
        {
            base.PostUpdateCustomerSettingType_SettingContextIsNull_ThrowArgumentNullException();
        }

        [Test]
        public override void PostUpdateCustomerSettingType_SettingTypeIsNull_ThrowArgumentNullException()
        {
            base.PostUpdateCustomerSettingType_SettingTypeIsNull_ThrowArgumentNullException();
        }

        [Test]
        public void PostUpdateCustomerSettingType_ResultIsSuccessful_ReturnUpdatePostionTakingSuccessfulResult()
        {
            //// Arrange
            var successResult = new SettingResult(Result.Success);

            //// Act
            var actualResult = SettingPostTasks.PostUpdateCustomerSettingType(
                SettingContext,
                1,
                new SettingType { SettingLevel = 1 },
                null,
                successResult);

            //// Assert
            Assert.AreEqual(Message.UpdateCommissionSuccessfully, actualResult.Message);
        }

        [Test]
        public void PostUpdateCustomerSettingType_ResultIsFailed_ExecuteTranslateMessageInMessageService()
        {
            //// Arrange
            var errorResult = new SettingResult(Result.DataError);
            _messageService.TranslateMessage(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>())
                .Returns(new MessageContent(string.Empty));
            byte settingLevel = 1;

            //// Act
            SettingPostTasks.PostUpdateCustomerSettingType(
                SettingContext,
                1,
                new SettingType
                {
                    SettingLevel = settingLevel
                },
                null,
                errorResult);

            //// Assert
            _messageService.Received().TranslateMessage(errorResult.MsgType, errorResult.Message, settingLevel, SettingContext.Language);
        }
    }
}