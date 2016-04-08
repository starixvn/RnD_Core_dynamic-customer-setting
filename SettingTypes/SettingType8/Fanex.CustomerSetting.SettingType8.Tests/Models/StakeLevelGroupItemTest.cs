namespace Fanex.CustomerSetting.SettingType8.Tests.Models
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using SettingType8.Models;

    public class StakeLevelGroupItemTest
    {
        [Test]
        public void ToXml_Always_ReturnValidXmlString()
        {
            // Arrange.
            var stakeLevel = new StakeLevelGroupItem
            {
                SportTypeId = 1,
                BetTypeId = 1,
                Groups = new List<LevelGroup>
                {
                    new LevelGroup
                    {
                        LevelId = 1,
                        LevelGroupId = 1,
                        Selected = true
                    },
                    new LevelGroup
                    {
                        LevelId = 2,
                        LevelGroupId = 2,
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