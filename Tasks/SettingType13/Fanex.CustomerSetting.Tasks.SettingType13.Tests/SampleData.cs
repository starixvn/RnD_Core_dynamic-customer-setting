namespace Fanex.CustomerSetting.Tasks.SettingType13.Tests
{
    using System.Collections.Generic;
    using CustomerSetting.SettingType13.Models;
    using Models;
    using Models.Enumerations;
    using NSubstitute;

    internal static class SampleData
    {
        public static ISetting CreateSampleSettingData()
        {
            ISetting setting = Substitute.For<ISetting>();

            setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    Id = AddSettingType13WarningTask.SettingType13Id,
                    DisplayMode = 1,
                    SettingItems = new List<ProductStatusItem>
                    {
                        new ProductStatusItem
                        {
                            SportTypeId = 1,
                            BetTypeId = 1,
                            IsDisabled = false,
                            Upline = new ProductStatusItem
                            {
                                IsDisabled = false
                            },
                            Downline = new ProductStatusItem
                            {
                                IsDisabled = false
                            }
                        }
                    }
                }
            };
            setting.CustomerLevel = Level.Master;
            setting.WarningMessage = null;

            return setting;
        }
    }
}