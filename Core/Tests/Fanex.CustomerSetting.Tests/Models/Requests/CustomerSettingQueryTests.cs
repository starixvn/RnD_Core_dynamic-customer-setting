namespace Fanex.CustomerSetting.Tests.Models.Requests
{
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class CustomerSettingQueryTests
    {
        private CustomerSettingQuery customerSettingQuery;

        [SetUp]
        public void SetUp()
        {
            customerSettingQuery = new CustomerSettingQuery(1, 1, 1, 0, 2);
        }

        [Test]
        public void CustomerId_SetToZero_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerSettingQuery.CustomerId = 0);
        }

        [Test]
        public void CustomerId_SetToNonZero_Success()
        {
            customerSettingQuery.CustomerId = 1;

            Assert.AreEqual(1, customerSettingQuery.CustomerId);
        }

        [Test]
        public void CustomerLevel_SetToCompanyLevel_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerSettingQuery.CustomerLevel = Level.Company);
        }

        [Test]
        public void CustomerLevel_SetToNonCompanyLevel_Success()
        {
            customerSettingQuery.CustomerLevel = Level.Super;
            Assert.AreEqual(Level.Super, customerSettingQuery.CustomerLevel);
            customerSettingQuery.CustomerLevel = Level.Master;
            Assert.AreEqual(Level.Master, customerSettingQuery.CustomerLevel);
            customerSettingQuery.CustomerLevel = Level.Agent;
            Assert.AreEqual(Level.Agent, customerSettingQuery.CustomerLevel);
            customerSettingQuery.CustomerLevel = Level.Member;
            Assert.AreEqual(Level.Member, customerSettingQuery.CustomerLevel);
        }

        [Test]
        public void ProductId_SetToZero_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => customerSettingQuery.ProductId = 0);
        }

        [Test]
        public void ProductId_SetToNonZero_Success()
        {
            customerSettingQuery.ProductId = 1;

            Assert.AreEqual(1, customerSettingQuery.ProductId);
        }

        [Test]
        public void UplineLevel_SetToNullWhenCustomerIsMember_ThrowArgumentNullException()
        {
            customerSettingQuery.CustomerLevel = Level.Member;
            Assert.Throws<ArgumentNullException>(() => customerSettingQuery.UplineLevel = null);
        }

        [Test]
        public void UplineLevel_SetToUplineWhenCustomerIsSuperMasterAgent_ThrowArgumentOutOfRangeException()
        {
            customerSettingQuery.CustomerLevel = Level.Super;
            Assert.Throws<ArgumentOutOfRangeException>(() => customerSettingQuery.UplineLevel = Level.Company);

            customerSettingQuery.CustomerLevel = Level.Master;
            Assert.Throws<ArgumentOutOfRangeException>(() => customerSettingQuery.UplineLevel = Level.Super);

            customerSettingQuery.CustomerLevel = Level.Agent;
            Assert.Throws<ArgumentOutOfRangeException>(() => customerSettingQuery.UplineLevel = Level.Master);
        }

        [Test]
        public void UplineLevel_SetToNullWhenCustomerIsSuperMasterAgent_Success()
        {
            customerSettingQuery.CustomerLevel = Level.Super;
            customerSettingQuery.UplineLevel = null;
            Assert.AreEqual(null, customerSettingQuery.UplineLevel);

            customerSettingQuery.CustomerLevel = Level.Master;
            customerSettingQuery.UplineLevel = null;
            Assert.AreEqual(null, customerSettingQuery.UplineLevel);

            customerSettingQuery.CustomerLevel = Level.Agent;
            customerSettingQuery.UplineLevel = null;
            Assert.AreEqual(null, customerSettingQuery.UplineLevel);
        }
    }
}