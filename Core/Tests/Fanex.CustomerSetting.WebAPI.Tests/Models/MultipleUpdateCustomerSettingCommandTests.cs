namespace Fanex.CustomerSetting.WebAPI.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using NUnit.Framework;
    using WebApi.Models.Request;

    [TestFixture]
    public class MultipleUpdateCustomerSettingCommandTests : UpdateSettingRequestTests
    {
        protected new MultipleUpdateSettingRequest Request { get; set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Request = new MultipleUpdateSettingRequest
            {
                Setting = SampleSetting,
                UplineLevel = Level.Agent.Value,
                CustomerIds = "123",
                CustomerNames = "supertest",
            };
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetMultipleUpdateCustomerSettingCommand_CustomerIdsIsNullOrEmpty_ThrowInvalidOperationException(string customerIds)
        {
            // Arrange.
            Request.CustomerIds = customerIds;

            // Act and Assert.
            Assert.Throws<InvalidOperationException>(() => Request.GetMultipleUpdateCustomerSettingCommand());
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetMultipleUpdateCustomerSettingCommand_CustomerNamesIsNullOrEmpty_ThrowInvalidOperationException(string customerNames)
        {
            // Arrange.
            Request.CustomerNames = customerNames;

            // Act and Assert.
            Assert.Throws<InvalidOperationException>(() => Request.GetMultipleUpdateCustomerSettingCommand());
        }

        [Test]
        public void GetMultipleUpdateCustomerSettingCommand_CustomerIdsAndCustomerNamesAreNotNull_ReturnObject()
        {
            // Act.
            var command = Request.GetMultipleUpdateCustomerSettingCommand();

            // Assert.
            Assert.NotNull(command);
        }

        [Test]
        public void GetMultipleUpdateCustomerSettingCommand_CustomerIdsAndCustomerNamesAreNotNull_ReturnTypeOfUpdateCustomerSettingCommand()
        {
            // Act.
            var command = Request.GetMultipleUpdateCustomerSettingCommand();

            // Assert.
            Assert.IsInstanceOf<MultipleUpdateCustomerSettingCommand>(command);
        }

        [Test]
        public void GetMultipleUpdateCustomerSettingCommand_CustomerIdsAndCustomerNamesAreNotNull_ReturnedObjectContainsExpectedData()
        {
            // Act.
            var command = Request.GetMultipleUpdateCustomerSettingCommand();

            // Assert.
            Assert.AreSame(SampleSetting, command.Setting);
            Assert.AreEqual(Level.Agent, command.UplineLevel);
            Assert.AreEqual(new List<int> { 123 }, command.CustomerIds);
            Assert.AreEqual(new List<string> { "supertest" }, command.CustomerNames);
        }
    }
}