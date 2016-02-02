namespace Fanex.CustomerSetting.SettingType7.Tests.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.SettingType7.Models;
    using NUnit.Framework;

    public class StakeLevelItemTest
    {
        [Test]
        public void ToXml_Always_ReturnValidXmlString()
        {
            // Arrange.
            var stakeLevel = new StakeLevelItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                StakeRanges = new List<StakeRange>
                {
                    new StakeRange
                    {
                        StakeLevelId = 1,
                        Selected = true
                    },
                    new StakeRange
                    {
                        StakeLevelId = 2,
                        Selected = false
                    }
                }
            };

            // Act.
            var actualXml = stakeLevel.ToXml();

            // Assert.
            var expectedXml = "<StakeLevel SportType=\"1\" BetType=\"1\" StakeLevelList=\"1\"/>";
            Assert.AreEqual(expectedXml, actualXml);
        }
    }
}