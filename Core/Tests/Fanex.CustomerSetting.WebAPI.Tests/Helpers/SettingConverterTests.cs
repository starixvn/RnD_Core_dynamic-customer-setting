namespace Fanex.CustomerSetting.WebAPI.Tests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NUnit.Framework;
    using WebApi.Helpers;
    using WebApi.Models.Request;

    [TestFixture]
    public class SettingConverterTests
    {
        private const string SampleDataFilePath = @"\SampleData\settings.json";

        private static Setting ExpectedData
        {
            get
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

                return new Setting(1, Level.Super, customerInfo, 255, null, new List<SettingType> { settingType });
            }
        }

        public static string ReadSampleData()
        {
            string sampleSetting = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + SampleDataFilePath);

            sampleSetting = sampleSetting.Replace("\"", "\\\"");

            return "{\"Setting\":\"" + sampleSetting + "\"}";
        }

        [Test]
        public void SerializeObject_Always_ReturnExpectedData()
        {
            // Act.
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                Converters = new List<JsonConverter>
                {
                    new SettingConverter(),
                    new SettingTypeConverter(),
                    new SettingItemConverter(),
                    new LevelConverter()
                }
            };
            var setting = JsonConvert.DeserializeObject<UpdateSettingRequest>(ReadSampleData(), jsonSerializerSettings);

            // Assert.
            string reserializedSettingJson = JsonConvert.SerializeObject(setting.Setting);
            string expectedSettingJson = JsonConvert.SerializeObject(ExpectedData);

            Assert.AreEqual(expectedSettingJson, reserializedSettingJson);
        }
    }
}