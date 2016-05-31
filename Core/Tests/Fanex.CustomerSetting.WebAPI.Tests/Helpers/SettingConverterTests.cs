namespace Fanex.CustomerSetting.WebAPI.Tests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;
    using Models.Enumerations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NUnit.Framework;
    using WebApi.Helpers;
    using WebApi.Models.Request;

    [TestFixture]
    public class SettingConverterTests
    {
        private const string SampleDataFilePath = @"\Helpers\sample-data.json";

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

        public static string[] ReadSampleData()
        {
            string sampleSetting = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + SampleDataFilePath);

            sampleSetting = sampleSetting.Replace("\"", "\\\"");

            return new string[] { "{\"Setting\":\"" + sampleSetting + "\"}" };
        }

        [Test, TestCaseSource(nameof(ReadSampleData))]
        public void SerializeObject_Always_ReturnExpectedData(string data)
        {
            // Act.
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                Converters = new List<JsonConverter>
                {
                    new SettingConverter(),
                    new SettingItemConverter(),
                    new LevelConverter()
                }
            };
            var setting = JsonConvert.DeserializeObject<UpdateSettingRequest>(data, jsonSerializerSettings);

            // Assert.
            string reserializedSettingJson = JsonConvert.SerializeObject(setting.Setting);
            string expectedSettingJson = JsonConvert.SerializeObject(ExpectedData);

            Assert.AreEqual(expectedSettingJson, reserializedSettingJson);
        }
    }
}