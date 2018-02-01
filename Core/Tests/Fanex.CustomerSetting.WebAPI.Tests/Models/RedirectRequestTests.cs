namespace Fanex.CustomerSetting.WebAPI.Tests.Models
{
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using NUnit.Framework;
    using WebApi.Models.Request;

    [TestFixture]
    public class RedirectRequestTests
    {
        private RedirectRequest _request;

        [SetUp]
        public void SetUp()
        {
            _request = new RedirectRequest
            {
                ProductId = 1,
                CustomerId = 123,
                CustomerLevel = Level.Member.Value,
                UplineLevel = Level.Agent.Value,
                SettingTypeId = 1,
                ActionId = 1,
                ActionType = (byte)ActionType.Update,
                JsonParameters = "{ \"param\": 1 }"
            };
        }

        [Test]
        public void ToRedirectCommand_Always_ReturnObject()
        {
            // Act.
            var command = _request.ToRedirectCommand();

            // Assert.
            Assert.NotNull(command);
        }

        [Test]
        public void ToRedirectCommand_Always_ReturnTypeOfRedirectCommand()
        {
            // Act.
            var command = _request.ToRedirectCommand();

            // Assert.
            Assert.IsAssignableFrom<RedirectCommand>(command);
        }

        [Test]
        public void ToRedirectCommand_Always_ReturnedObjectContainsExpectedData()
        {
            // Act.
            var command = _request.ToRedirectCommand();

            // Assert.
            Assert.AreEqual(1, command.SettingTypeId);
            Assert.AreEqual(1, command.ActionId);
            Assert.AreEqual(ActionType.Update, command.ActionType);
            Assert.AreEqual("{ \"param\": 1 }", command.JsonParameters);
        }
    }
}