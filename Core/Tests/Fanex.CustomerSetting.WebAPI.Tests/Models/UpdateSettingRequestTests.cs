namespace Fanex.CustomerSetting.WebAPI.Tests.Models
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using NUnit.Framework;
    using WebApi.Models.Request;

    [TestFixture]
    public class UpdateSettingRequestTests
    {
        private static Setting _sampleSetting;

        protected static Setting SampleSetting
        {
            get
            {
                if (_sampleSetting == null)
                {
                    var customerInfo = new CustomerInfo
                    {
                        UserName = "SA-RM",
                        FirstName = "---",
                        LastName = "---",
                        Phone = "-",
                        MobilePhone = "-",
                        Fax = "-",
                        Email = "test@test.com"
                    };
                    var settingItem = new SettingItemDefault
                    {
                        SportTypeId = 121,
                        SportTypeName = "Sport Type 121",
                        BetTypeId = 30001,
                        BetTypeName = "Bet Type 30001"
                    };
                    var settingType = new SettingType
                    {
                        Id = 1,
                        Name = "Setting Type 1",
                        SettingLevel = 1,
                        DisplayOrder = 1,
                        DisplayMode = 1,
                        SettingItems = new List<ISettingItem>
                    {
                        settingItem
                    }
                    };

                    _sampleSetting = new Setting(1, Level.Member, customerInfo, 255, null, new List<SettingType> { settingType });
                }

                return _sampleSetting;
            }
        }

        protected virtual UpdateSettingRequest Request { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            Request = new UpdateSettingRequest
            {
                Setting = SampleSetting,
                UplineLevel = Level.Agent.Value
            };
        }

        [Test]
        public void GetUpdateCustomerSettingCommand_Always_ReturnObject()
        {
            // Act.
            var command = this.Request.GetUpdateCustomerSettingCommand();

            // Assert.
            Assert.NotNull(command);
        }

        [Test]
        public void GetUpdateCustomerSettingCommand_Always_ReturnTypeOfUpdateCustomerSettingCommand()
        {
            // Act.
            var command = this.Request.GetUpdateCustomerSettingCommand();

            // Assert.
            Assert.IsInstanceOf<UpdateCustomerSettingCommand>(command);
        }

        [Test]
        public void GetUpdateCustomerSettingCommand_Always_ReturnedObjectContainsExpectedData()
        {
            // Act.
            var command = this.Request.GetUpdateCustomerSettingCommand();

            // Assert.
            Assert.AreSame(SampleSetting, command.Setting);
            Assert.AreEqual(Level.Agent, command.UplineLevel);
        }
    }
}