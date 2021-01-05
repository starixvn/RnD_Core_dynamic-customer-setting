namespace Fanex.CustomerSetting.Tests.Enumeration
{
    using Fanex.CustomerSetting.Models.Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class LevelTests
    {
        [Test]
        public void IsCompanyLevel_Company_ReturnTrue()
        {
            Assert.IsTrue(Level.Company.IsCompanyLevel());
        }

        [Test]
        public void IsNotCompanyLevel_Company_ReturnFalse()
        {
            Assert.IsFalse(Level.Company.IsNotCompanyLevel());
        }

        [Test]
        public void IsCompanyLevel_NotCompany_ReturnFalse()
        {
            Assert.IsFalse(Level.Super.IsCompanyLevel());
            Assert.IsFalse(Level.Master.IsCompanyLevel());
            Assert.IsFalse(Level.Agent.IsCompanyLevel());
            Assert.IsFalse(Level.Member.IsCompanyLevel());
        }

        [Test]
        public void IsSuperLevel_Super_ReturnTrue()
        {
            Assert.IsTrue(Level.Super.IsSuperLevel());
        }

        [Test]
        public void IsNotSuperLevel_Super_ReturnFalse()
        {
            Assert.IsFalse(Level.Super.IsNotSuperLevel());
        }

        [Test]
        public void IsSuperLevel_NotSuper_ReturnFalse()
        {
            Assert.IsFalse(Level.Company.IsSuperLevel());
            Assert.IsFalse(Level.Master.IsSuperLevel());
            Assert.IsFalse(Level.Agent.IsSuperLevel());
            Assert.IsFalse(Level.Member.IsSuperLevel());
        }

        [Test]
        public void IsMasterLevel_Master_ReturnTrue()
        {
            Assert.IsTrue(Level.Master.IsMasterLevel());
        }

        [Test]
        public void IsNotMasterLevel_Master_ReturnFalse()
        {
            Assert.IsFalse(Level.Master.IsNotMasterLevel());
        }

        [Test]
        public void IsMasterLevel_NotMaster_ReturnFalse()
        {
            Assert.IsFalse(Level.Company.IsMasterLevel());
            Assert.IsFalse(Level.Super.IsMasterLevel());
            Assert.IsFalse(Level.Agent.IsMasterLevel());
            Assert.IsFalse(Level.Member.IsMasterLevel());
        }

        [Test]
        public void IsAgentLevel_Agent_ReturnTrue()
        {
            Assert.IsTrue(Level.Agent.IsAgentLevel());
        }

        [Test]
        public void IsNotAgentLevel_Agent_ReturnFalse()
        {
            Assert.IsFalse(Level.Agent.IsNotAgentLevel());
        }

        [Test]
        public void IsAgentLevel_NotAgent_ReturnFalse()
        {
            Assert.IsFalse(Level.Company.IsAgentLevel());
            Assert.IsFalse(Level.Super.IsAgentLevel());
            Assert.IsFalse(Level.Master.IsAgentLevel());
            Assert.IsFalse(Level.Member.IsAgentLevel());
        }

        [Test]
        public void IsMemberLevel_Member_ReturnTrue()
        {
            Assert.IsTrue(Level.Member.IsMemberLevel());
        }

        [Test]
        public void IsMemberLevel_Member_ReturnFalse()
        {
            Assert.IsFalse(Level.Member.IsNotMemberLevel());
        }

        [Test]
        public void IsMemberLevel_NotMember_ReturnFalse()
        {
            Assert.IsFalse(Level.Company.IsMemberLevel());
            Assert.IsFalse(Level.Super.IsMemberLevel());
            Assert.IsFalse(Level.Master.IsMemberLevel());
            Assert.IsFalse(Level.Agent.IsMemberLevel());
        }

        [Test]
        public void IsDirectUplineOf_Null_ReturnFalse()
        {
            Assert.IsFalse(Level.Super.IsDirectUplineOf(null));
        }

        [Test]
        public void IsDirectUplineOf_ValidValue_ReturnTrue()
        {
            Assert.IsTrue(Level.Company.IsDirectUplineOf(Level.Super.Value));
            Assert.IsTrue(Level.Super.IsDirectUplineOf(Level.Master.Value));
            Assert.IsTrue(Level.Master.IsDirectUplineOf(Level.Agent.Value));
            Assert.IsTrue(Level.Agent.IsDirectUplineOf(Level.Member.Value));

            Assert.IsTrue(Level.Company.IsDirectUplineOf(Level.Super));
            Assert.IsTrue(Level.Super.IsDirectUplineOf(Level.Master));
            Assert.IsTrue(Level.Master.IsDirectUplineOf(Level.Agent));
            Assert.IsTrue(Level.Agent.IsDirectUplineOf(Level.Member));
        }

        [Test]
        public void IsDirectUplineOf_InvalidValue_ReturnFalse()
        {
            Assert.IsFalse(Level.Company.IsDirectUplineOf(Level.Company.Value));
            Assert.IsFalse(Level.Company.IsDirectUplineOf(Level.Master.Value));
            Assert.IsFalse(Level.Company.IsDirectUplineOf(Level.Agent.Value));
            Assert.IsFalse(Level.Company.IsDirectUplineOf(Level.Member.Value));

            Assert.IsFalse(Level.Super.IsDirectUplineOf(Level.Company.Value));
            Assert.IsFalse(Level.Super.IsDirectUplineOf(Level.Super.Value));
            Assert.IsFalse(Level.Super.IsDirectUplineOf(Level.Agent.Value));
            Assert.IsFalse(Level.Super.IsDirectUplineOf(Level.Member.Value));

            Assert.IsFalse(Level.Master.IsDirectUplineOf(Level.Company.Value));
            Assert.IsFalse(Level.Master.IsDirectUplineOf(Level.Super.Value));
            Assert.IsFalse(Level.Master.IsDirectUplineOf(Level.Master.Value));
            Assert.IsFalse(Level.Master.IsDirectUplineOf(Level.Member.Value));

            Assert.IsFalse(Level.Agent.IsDirectUplineOf(Level.Company.Value));
            Assert.IsFalse(Level.Agent.IsDirectUplineOf(Level.Super.Value));
            Assert.IsFalse(Level.Agent.IsDirectUplineOf(Level.Master.Value));
            Assert.IsFalse(Level.Agent.IsDirectUplineOf(Level.Agent.Value));

            Assert.IsFalse(Level.Member.IsDirectUplineOf(Level.Company.Value));
            Assert.IsFalse(Level.Member.IsDirectUplineOf(Level.Super.Value));
            Assert.IsFalse(Level.Member.IsDirectUplineOf(Level.Master.Value));
            Assert.IsFalse(Level.Member.IsDirectUplineOf(Level.Agent.Value));
            Assert.IsFalse(Level.Member.IsDirectUplineOf(Level.Member.Value));


            Assert.IsFalse(Level.Company.IsDirectUplineOf(Level.Company));
            Assert.IsFalse(Level.Company.IsDirectUplineOf(Level.Master));
            Assert.IsFalse(Level.Company.IsDirectUplineOf(Level.Agent));
            Assert.IsFalse(Level.Company.IsDirectUplineOf(Level.Member));

            Assert.IsFalse(Level.Super.IsDirectUplineOf(Level.Company));
            Assert.IsFalse(Level.Super.IsDirectUplineOf(Level.Super));
            Assert.IsFalse(Level.Super.IsDirectUplineOf(Level.Agent));
            Assert.IsFalse(Level.Super.IsDirectUplineOf(Level.Member));

            Assert.IsFalse(Level.Master.IsDirectUplineOf(Level.Company));
            Assert.IsFalse(Level.Master.IsDirectUplineOf(Level.Super));
            Assert.IsFalse(Level.Master.IsDirectUplineOf(Level.Master));
            Assert.IsFalse(Level.Master.IsDirectUplineOf(Level.Member));

            Assert.IsFalse(Level.Agent.IsDirectUplineOf(Level.Company));
            Assert.IsFalse(Level.Agent.IsDirectUplineOf(Level.Super));
            Assert.IsFalse(Level.Agent.IsDirectUplineOf(Level.Master));
            Assert.IsFalse(Level.Agent.IsDirectUplineOf(Level.Agent));

            Assert.IsFalse(Level.Member.IsDirectUplineOf(Level.Company));
            Assert.IsFalse(Level.Member.IsDirectUplineOf(Level.Super));
            Assert.IsFalse(Level.Member.IsDirectUplineOf(Level.Master));
            Assert.IsFalse(Level.Member.IsDirectUplineOf(Level.Agent));
            Assert.IsFalse(Level.Member.IsDirectUplineOf(Level.Member));
        }



        [Test]
        public void IsNotDirectUplineOf_Null_ReturnTrue()
        {
            Assert.IsTrue(Level.Super.IsNotDirectUplineOf(null));
        }

        [Test]
        public void IsNotDirectUplineOf_InvalidValue_ReturnFalse()
        {
            Assert.IsFalse(Level.Company.IsNotDirectUplineOf(Level.Super.Value));
            Assert.IsFalse(Level.Company.IsNotDirectUplineOf(Level.Super.Value));
            Assert.IsFalse(Level.Super.IsNotDirectUplineOf(Level.Master.Value));
            Assert.IsFalse(Level.Master.IsNotDirectUplineOf(Level.Agent.Value));
            Assert.IsFalse(Level.Agent.IsNotDirectUplineOf(Level.Member.Value));
                     
            Assert.IsFalse(Level.Company.IsNotDirectUplineOf(Level.Super));
            Assert.IsFalse(Level.Company.IsNotDirectUplineOf(Level.Super));
            Assert.IsFalse(Level.Super.IsNotDirectUplineOf(Level.Master));
            Assert.IsFalse(Level.Master.IsNotDirectUplineOf(Level.Agent));
            Assert.IsFalse(Level.Agent.IsNotDirectUplineOf(Level.Member));
        }

        [Test]
        public void IsNotDirectUplineOf_ValidValue_ReturnTrue()
        {
            Assert.IsTrue(Level.Company.IsNotDirectUplineOf(Level.Company.Value));
            Assert.IsTrue(Level.Company.IsNotDirectUplineOf(Level.Master.Value));
            Assert.IsTrue(Level.Company.IsNotDirectUplineOf(Level.Agent.Value));
            Assert.IsTrue(Level.Company.IsNotDirectUplineOf(Level.Member.Value));
                     
            Assert.IsTrue(Level.Super.IsNotDirectUplineOf(Level.Company.Value));
            Assert.IsTrue(Level.Super.IsNotDirectUplineOf(Level.Super.Value));
            Assert.IsTrue(Level.Super.IsNotDirectUplineOf(Level.Agent.Value));
            Assert.IsTrue(Level.Super.IsNotDirectUplineOf(Level.Member.Value));
                     
            Assert.IsTrue(Level.Master.IsNotDirectUplineOf(Level.Company.Value));
            Assert.IsTrue(Level.Master.IsNotDirectUplineOf(Level.Super.Value));
            Assert.IsTrue(Level.Master.IsNotDirectUplineOf(Level.Master.Value));
            Assert.IsTrue(Level.Master.IsNotDirectUplineOf(Level.Member.Value));
                     
            Assert.IsTrue(Level.Agent.IsNotDirectUplineOf(Level.Company.Value));
            Assert.IsTrue(Level.Agent.IsNotDirectUplineOf(Level.Super.Value));
            Assert.IsTrue(Level.Agent.IsNotDirectUplineOf(Level.Master.Value));
            Assert.IsTrue(Level.Agent.IsNotDirectUplineOf(Level.Agent.Value));
                     
            Assert.IsTrue(Level.Member.IsNotDirectUplineOf(Level.Company.Value));
            Assert.IsTrue(Level.Member.IsNotDirectUplineOf(Level.Super.Value));
            Assert.IsTrue(Level.Member.IsNotDirectUplineOf(Level.Master.Value));
            Assert.IsTrue(Level.Member.IsNotDirectUplineOf(Level.Agent.Value));
            Assert.IsTrue(Level.Member.IsNotDirectUplineOf(Level.Member.Value));
                     
                     
            Assert.IsTrue(Level.Company.IsNotDirectUplineOf(Level.Company));
            Assert.IsTrue(Level.Company.IsNotDirectUplineOf(Level.Master));
            Assert.IsTrue(Level.Company.IsNotDirectUplineOf(Level.Agent));
            Assert.IsTrue(Level.Company.IsNotDirectUplineOf(Level.Member));
                     
            Assert.IsTrue(Level.Super.IsNotDirectUplineOf(Level.Company));
            Assert.IsTrue(Level.Super.IsNotDirectUplineOf(Level.Super));
            Assert.IsTrue(Level.Super.IsNotDirectUplineOf(Level.Agent));
            Assert.IsTrue(Level.Super.IsNotDirectUplineOf(Level.Member));
                     
            Assert.IsTrue(Level.Master.IsNotDirectUplineOf(Level.Company));
            Assert.IsTrue(Level.Master.IsNotDirectUplineOf(Level.Super));
            Assert.IsTrue(Level.Master.IsNotDirectUplineOf(Level.Master));
            Assert.IsTrue(Level.Master.IsNotDirectUplineOf(Level.Member));
                     
            Assert.IsTrue(Level.Agent.IsNotDirectUplineOf(Level.Company));
            Assert.IsTrue(Level.Agent.IsNotDirectUplineOf(Level.Super));
            Assert.IsTrue(Level.Agent.IsNotDirectUplineOf(Level.Master));
            Assert.IsTrue(Level.Agent.IsNotDirectUplineOf(Level.Agent));
                     
            Assert.IsTrue(Level.Member.IsNotDirectUplineOf(Level.Company));
            Assert.IsTrue(Level.Member.IsNotDirectUplineOf(Level.Super));
            Assert.IsTrue(Level.Member.IsNotDirectUplineOf(Level.Master));
            Assert.IsTrue(Level.Member.IsNotDirectUplineOf(Level.Agent));
            Assert.IsTrue(Level.Member.IsNotDirectUplineOf(Level.Member));
        }



        [Test]
        public void IsDirectDownlineOf_Null_ReturnFalse()
        {
            Assert.IsFalse(Level.Super.IsDirectDownlineOf(null));
        }

        [Test]
        public void IsDirectDownlineOf_ValidValue_ReturnTrue()
        {
            Assert.IsTrue(Level.Super.IsDirectDownlineOf(Level.Company));
            Assert.IsTrue(Level.Master.IsDirectDownlineOf(Level.Super));
            Assert.IsTrue(Level.Agent.IsDirectDownlineOf(Level.Master));
            Assert.IsTrue(Level.Member.IsDirectDownlineOf(Level.Agent));
        }

        [Test]
        public void IsDirectDownlineOf_InvalidValue_ReturnFalse()
        {
            Assert.IsFalse(Level.Company.IsDirectDownlineOf(Level.Company));
            Assert.IsFalse(Level.Company.IsDirectDownlineOf(Level.Master));
            Assert.IsFalse(Level.Company.IsDirectDownlineOf(Level.Agent));
            Assert.IsFalse(Level.Company.IsDirectDownlineOf(Level.Member));

            Assert.IsFalse(Level.Super.IsDirectDownlineOf(Level.Super));
            Assert.IsFalse(Level.Super.IsDirectDownlineOf(Level.Master));
            Assert.IsFalse(Level.Super.IsDirectDownlineOf(Level.Agent));
            Assert.IsFalse(Level.Super.IsDirectDownlineOf(Level.Member));

            Assert.IsFalse(Level.Master.IsDirectDownlineOf(Level.Company));
            Assert.IsFalse(Level.Master.IsDirectDownlineOf(Level.Master));
            Assert.IsFalse(Level.Master.IsDirectDownlineOf(Level.Agent));
            Assert.IsFalse(Level.Master.IsDirectDownlineOf(Level.Member));

            Assert.IsFalse(Level.Agent.IsDirectDownlineOf(Level.Company));
            Assert.IsFalse(Level.Agent.IsDirectDownlineOf(Level.Super));
            Assert.IsFalse(Level.Agent.IsDirectDownlineOf(Level.Agent));
            Assert.IsFalse(Level.Agent.IsDirectDownlineOf(Level.Member));

            Assert.IsFalse(Level.Member.IsDirectDownlineOf(Level.Company));
            Assert.IsFalse(Level.Member.IsDirectDownlineOf(Level.Super));
            Assert.IsFalse(Level.Member.IsDirectDownlineOf(Level.Master));
            Assert.IsFalse(Level.Member.IsDirectDownlineOf(Level.Member));
        }


        [Test]
        public void IsNotDirectDownlineOf_Null_ReturnTrue()
        {
            Assert.IsTrue(Level.Super.IsNotDirectDownlineOf(null));
        }

        [Test]
        public void IsNotDirectDownlineOf_ValidValue_ReturnFalse()
        {
            Assert.IsFalse(Level.Super.IsNotDirectDownlineOf(Level.Company));
            Assert.IsFalse(Level.Master.IsNotDirectDownlineOf(Level.Super));
            Assert.IsFalse(Level.Agent.IsNotDirectDownlineOf(Level.Master));
            Assert.IsFalse(Level.Member.IsNotDirectDownlineOf(Level.Agent));
        }

        [Test]
        public void IsNotDirectDownlineOf_InvalidValue_ReturnTrue()
        {
            Assert.IsTrue(Level.Company.IsNotDirectDownlineOf(Level.Company));
            Assert.IsTrue(Level.Company.IsNotDirectDownlineOf(Level.Super));
            Assert.IsTrue(Level.Company.IsNotDirectDownlineOf(Level.Master));
            Assert.IsTrue(Level.Company.IsNotDirectDownlineOf(Level.Agent));
            Assert.IsTrue(Level.Company.IsNotDirectDownlineOf(Level.Member));
                     
            Assert.IsTrue(Level.Super.IsNotDirectDownlineOf(Level.Super));
            Assert.IsTrue(Level.Super.IsNotDirectDownlineOf(Level.Master));
            Assert.IsTrue(Level.Super.IsNotDirectDownlineOf(Level.Agent));
            Assert.IsTrue(Level.Super.IsNotDirectDownlineOf(Level.Member));
                     
            Assert.IsTrue(Level.Master.IsNotDirectDownlineOf(Level.Company));
            Assert.IsTrue(Level.Master.IsNotDirectDownlineOf(Level.Master));
            Assert.IsTrue(Level.Master.IsNotDirectDownlineOf(Level.Agent));
            Assert.IsTrue(Level.Master.IsNotDirectDownlineOf(Level.Member));
                     
            Assert.IsTrue(Level.Agent.IsNotDirectDownlineOf(Level.Company));
            Assert.IsTrue(Level.Agent.IsNotDirectDownlineOf(Level.Super));
            Assert.IsTrue(Level.Agent.IsNotDirectDownlineOf(Level.Agent));
            Assert.IsTrue(Level.Agent.IsNotDirectDownlineOf(Level.Member));
                     
            Assert.IsTrue(Level.Member.IsNotDirectDownlineOf(Level.Company));
            Assert.IsTrue(Level.Member.IsNotDirectDownlineOf(Level.Super));
            Assert.IsTrue(Level.Member.IsNotDirectDownlineOf(Level.Master));
            Assert.IsTrue(Level.Member.IsNotDirectDownlineOf(Level.Member));
        }
    }
}