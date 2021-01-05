namespace Fanex.CustomerSetting.SettingType18.Tests.Models
{
    using System.Collections.Generic;
    using CustomerSetting.Models.Constants;
    using CustomerSetting.Models.Enumerations;
    using NUnit.Framework;
    using SettingType18.Models;

    [TestFixture]
    public class PTSettingTypeTests
    {
        private PTSettingType _settingType;

        [SetUp]
        public void Setup()
        {
            _settingType = new PTSettingType
            {
                DisplayMode = DisplayModes.Editable
            };
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_AdminEditsAnyCustomerLevel_ReturnTrue()
        {
            // Arrange.
            var actorLevel = Level.Company;
            var anyCustomerLevel = Level.Agent;

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, anyCustomerLevel);

            // Assert.
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_EditSuperButSettingItemModelIsNotSuperPTItem_ReturnFalse()
        {
            // Arrange.
            var actorLevel = Level.Super;
            var customerLevel = Level.Super;

            _settingType.SettingItems = new List<PTItem>
            {
                new PTItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_EditSuperAndSettingItemModelIsSuperPTItem_ReturnTrue()
        {
            // Arrange.
            var actorLevel = Level.Super;
            var customerLevel = Level.Super;

            _settingType.SettingItems = new List<SuperPTItem>
            {
                new SuperPTItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_EditMasterButSettingItemModelIsNotMasterPTItem_ReturnFalse()
        {
            // Arrange.
            var actorLevel = Level.Super;
            var customerLevel = Level.Master;

            _settingType.SettingItems = new List<PTItem>
            {
                new PTItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_EditMasterAndSettingItemModelIsMasterPTItem_ReturnTrue()
        {
            // Arrange.
            var actorLevel = Level.Super;
            var customerLevel = Level.Master;

            _settingType.SettingItems = new List<MasterPTItem>
            {
                new MasterPTItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_EditAgentButSettingItemModelIsNotAgentPTItem_ReturnFalse()
        {
            // Arrange.
            var actorLevel = Level.Master;
            var customerLevel = Level.Agent;

            _settingType.SettingItems = new List<PTItem>
            {
                new PTItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_EditAgentAndSettingItemModelIsAgentPTItem_ReturnTrue()
        {
            // Arrange.
            var actorLevel = Level.Master;
            var customerLevel = Level.Agent;

            _settingType.SettingItems = new List<AgentPTItem>
            {
                new AgentPTItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_SuperEditsMemberButSettingItemModelIsNotSuperPTOnMemberItem_ReturnFalse()
        {
            // Arrange.
            var actorLevel = Level.Super;
            var customerLevel = Level.Member;

            _settingType.SettingItems = new List<PTItem>
            {
                new PTItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_SuperEditsMemberAndSettingItemModelIsSuperPTOnMemberItem_ReturnTrue()
        {
            // Arrange.
            var actorLevel = Level.Super;
            var customerLevel = Level.Member;

            _settingType.SettingItems = new List<SuperPTOnMemberItem>
            {
                new SuperPTOnMemberItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_MasterEditsMemberButSettingItemModelIsNotMasterPTOnMemberItem_ReturnFalse()
        {
            // Arrange.
            var actorLevel = Level.Master;
            var customerLevel = Level.Member;

            _settingType.SettingItems = new List<PTItem>
            {
                new PTItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_MasterEditsMemberAndSettingItemModelIsMasterPTOnMemberItem_ReturnTrue()
        {
            // Arrange.
            var actorLevel = Level.Master;
            var customerLevel = Level.Member;

            _settingType.SettingItems = new List<MasterPTOnMemberItem>
            {
                new MasterPTOnMemberItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_AgentEditsMemberButSettingItemModelIsNotAgentPTOnMemberItem_ReturnFalse()
        {
            // Arrange.
            var actorLevel = Level.Agent;
            var customerLevel = Level.Member;

            _settingType.SettingItems = new List<PTItem>
            {
                new PTItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_AgentEditsMemberAndSettingItemModelIsAgentPTOnMemberItem_ReturnTrue()
        {
            // Arrange.
            var actorLevel = Level.Agent;
            var customerLevel = Level.Member;

            _settingType.SettingItems = new List<AgentPTOnMemberItem>
            {
                new AgentPTOnMemberItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void SatisfyCustomerLevelConstraint_InvalidActor_ReturnFalse()
        {
            // Arrange.
            var actorLevel = Level.Company;
            var customerLevel = Level.Member;

            _settingType.SettingItems = new List<AgentPTOnMemberItem>
            {
                new AgentPTOnMemberItem()
            };

            // Act.
            var actualResult = _settingType.SatisfyCustomerLevelConstraint(actorLevel, customerLevel);

            // Assert.
            Assert.IsTrue(actualResult);
        }
    }
}