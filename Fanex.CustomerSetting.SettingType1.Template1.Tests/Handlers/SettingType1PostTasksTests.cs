namespace Fanex.CustomerSetting.SettingType1.Template1.Tests.Handlers
{
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType1.Template1.Handlers;
    using Fanex.CustomerSetting.Tests.Handlers;
    using Fanex.CustomerSetting.Tests.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingType1PostTasksTests : SettingPostTasksTests
    {
        private IMessageService _messageService;

        [SetUp]
        public void Setup()
        {
            SettingContext = new SettingContextStub();
            ResourceService = Substitute.For<IResourceService>();
            _messageService = Substitute.For<IMessageService>();
            SettingPostTasks = new SettingType1PostTasks(_messageService, ResourceService);
        }

        [Test]
        public void PostUpdateCustomerSettingType_ResultIsSuccessful_ReturnUpdatePostionTakingSuccessfulResult()
        {
            //// Arrange
            var successResult = new SettingResult(Result.Success);

            //// Act
            var actualResult = SettingPostTasks.PostUpdateCustomerSettingType(
                SettingContext,
                new CustomerInfo(),
                new SettingType { SettingLevel = 1 },
                null,
                successResult);

            //// Assert
            Assert.AreEqual(Message.UpdatePositionTakingSuccessfully, actualResult.Message);
        }

        [Test]
        public void PostUpdateCustomerSettingType_ResultIsFailed_ExecuteTranslateMessageInMessageService()
        {
            //// Arrange
            var errorResult = new SettingResult(Result.SystemError);
            _messageService.TranslateMessage(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<byte>(), Arg.Any<string>())
                .Returns(new MessageContent(string.Empty));
            byte settingLevel = 1;

            //// Act
            SettingPostTasks.PostUpdateCustomerSettingType(
                SettingContext,
                new CustomerInfo(),
                new SettingType { SettingLevel = settingLevel },
                null,
                errorResult);

            //// Assert
            _messageService.Received().TranslateMessage(errorResult.MsgType, errorResult.Message, settingLevel, SettingContext.Language);
        }
    }
}