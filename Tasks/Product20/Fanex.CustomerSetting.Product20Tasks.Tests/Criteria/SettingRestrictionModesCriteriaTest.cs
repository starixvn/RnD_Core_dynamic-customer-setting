namespace Fanex.CustomerSetting.Product20Tasks.Tests.Criteria
{
    using Fanex.CustomerSetting.Tasks.Product20.Criteria;
    using NUnit.Framework;

    public class SettingRestrictionModesCriteriaTest
    {
        private SettingRestrictionModesCriteria _criteria;

        [SetUp]
        public void SetUp()
        {
            _criteria = new SettingRestrictionModesCriteria
            {
                SystemId = 1,
                TargetId = 1,
                TargetRoleId = 1,
                ActorRoleId = 1
            };
        }

        [Test]
        public void GetSettingKey_Always_ConfiguredKey()
        {
            // Assert.
            Assert.AreEqual("Acc_CustSetting_UpdateRestrictionSelect", _criteria.GetSettingKey());
        }

        [Test]
        public void IsValid_SystemIdIsLessThanOrEqualToZero_ReturnFalse()
        {
            // Arrange.
            _criteria.SystemId = 0;

            // Assert.
            Assert.IsFalse(_criteria.IsValid());
        }

        [Test]
        public void IsValid_TargetIdIsLessThanOrEqualToZero_ReturnFalse()
        {
            // Arrange.
            _criteria.TargetId = 0;

            // Assert.
            Assert.IsFalse(_criteria.IsValid());
        }

        [Test]
        public void IsValid_TargetRoleIdIsLessThanOrEqualToZero_ReturnFalse()
        {
            // Arrange.
            _criteria.TargetRoleId = 0;

            // Assert.
            Assert.IsFalse(_criteria.IsValid());
        }

        [Test]
        public void IsValid_ActorRoleIdIsLessThanOrEqualToZero_ReturnFalse()
        {
            // Arrange.
            _criteria.ActorRoleId = 0;

            // Assert.
            Assert.IsFalse(_criteria.IsValid());
        }
    }
}