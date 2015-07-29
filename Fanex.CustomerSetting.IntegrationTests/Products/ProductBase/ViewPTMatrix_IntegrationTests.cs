namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.IntegrationTests.Products.Product255;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Services;
    using Fanex.CustomerSetting.Matrix.Services.Impl;
    using Fanex.Data.Configuration;
    using NUnit.Framework;

    public class ViewPTMatrix_IntegrationTests
    {
        private ICustomerService _customerService;
        private IPTMatrixService _positionTakingMatrixService;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
            _customerService = new CustomerService();
            _positionTakingMatrixService = new PTMatrixService();
        }

        protected void ViewSuperPTMatrix(
            byte productId = Product255Constants.ProductId,
            int superId = SettingInfo.SuperId,
            string superName = SettingInfo.SuperUserName)
        {
            //// Act
            var superLine = _customerService.GetCustomerLine(superName);
            var superPTMatrix = _positionTakingMatrixService.GetPTMatrix(productId, superName);

            //// Assert
            //// Customer Line info
            Assert.AreEqual(1, superLine.Count());

            AssertSuperInfo(superId, superName, superLine);

            //// PT Matrix
            Assert.IsNotEmpty(superPTMatrix.PTItemMatrices);
            Assert.Greater(superPTMatrix.SettingLevel, 0);
            Assert.AreEqual(1, superPTMatrix.PTItemMatrices.First().PTItemMatrixRows.Count());

            AssertSuperPTMatrixRow(superPTMatrix);
        }

        private void AssertSuperPTMatrixRow(PTMatrix superPTMatrix)
        {
            var superPTRow = superPTMatrix.PTItemMatrices.First().PTItemMatrixRows.First();
            Assert.Greater(superPTRow.SuperPT, 0);
            Assert.Greater(superPTRow.MasterPT, 0);
            Assert.Greater(superPTRow.AgentPT, 0);
            Assert.GreaterOrEqual(superPTRow.MinPT, 0);
            Assert.IsTrue(superPTRow.CustomerLevel.IsSuperLevel());
        }

        private void AssertSuperInfo(int superId, string superName, IEnumerable<Customer> superLine)
        {
            var super = superLine.First();
            Assert.AreEqual(superId, super.Id);
            Assert.AreEqual(superName, super.Name);
        }

        protected void ViewMasterPTMatrix(
            byte productId = Product255Constants.ProductId,
            int superId = SettingInfo.SuperId,
            string superName = SettingInfo.SuperUserName,
            int masterId = SettingInfo.MasterId,
            string masterName = SettingInfo.MasterUserName)
        {
            //// Act
            var masterLine = _customerService.GetCustomerLine(masterName);
            var masterPTMatrix = _positionTakingMatrixService.GetPTMatrix(productId, masterName);

            //// Assert
            //// Customer Line info
            Assert.AreEqual(2, masterLine.Count());

            AssertSuperInfo(superId, superName, masterLine);

            AssertMasterInfo(masterId, masterName, masterLine);

            //// PT Matrix
            Assert.IsNotEmpty(masterPTMatrix.PTItemMatrices);

            AssertSuperPTMatrixRow(masterPTMatrix);

            AssertMasterPTMatrixRow(masterPTMatrix);
        }

        private void AssertMasterPTMatrixRow(PTMatrix masterPTMatrix)
        {
            var masterPTRow = masterPTMatrix.PTItemMatrices.First().PTItemMatrixRows.ElementAt(1);

            Assert.IsTrue(masterPTRow.CustomerLevel.IsMasterLevel());

            if (masterPTMatrix.IsAllowMasterAutoPT && masterPTRow.IsAutoMasterPT.Value)
            {
                Assert.Greater(masterPTRow.AutoMasterPT, 0);
            }
            else
            {
                Assert.Greater(masterPTRow.MasterPT, 0);
            }

            Assert.GreaterOrEqual(masterPTRow.MinPT, 0);
        }

        private void AssertMasterInfo(int masterId, string masterName, IEnumerable<Customer> masterLine)
        {
            var master = masterLine.ElementAt(1);
            Assert.AreEqual(masterId, master.Id);
            Assert.AreEqual(masterName, master.Name);
        }

        protected void ViewAgentPTMatrix(
            byte productId = Product255Constants.ProductId,
            int superId = SettingInfo.SuperId,
            string superName = SettingInfo.SuperUserName,
            int masterId = SettingInfo.MasterId,
            string masterName = SettingInfo.MasterUserName,
            int agentId = SettingInfo.AgentId,
            string agentName = SettingInfo.AgentUserName)
        {
            //// Act
            var agentLine = _customerService.GetCustomerLine(agentName);
            var agentPTMatrix = _positionTakingMatrixService.GetPTMatrix(productId, agentName);

            //// Assert
            //// Customer Line info
            Assert.AreEqual(3, agentLine.Count());

            AssertSuperInfo(superId, superName, agentLine);

            AssertMasterInfo(masterId, masterName, agentLine);

            AssertAgentInfo(agentId, agentName, agentLine);

            //// PT Matrix
            Assert.IsNotEmpty(agentPTMatrix.PTItemMatrices);

            AssertSuperPTMatrixRow(agentPTMatrix);

            AssertMasterPTMatrixRow(agentPTMatrix);

            AssertAgentPTMatrixRow(agentPTMatrix);
        }

        private void AssertAgentPTMatrixRow(PTMatrix agentPTMatrix)
        {
            var agentPTRow = agentPTMatrix.PTItemMatrices.First().PTItemMatrixRows.ElementAt(2);

            if (agentPTRow.IsAutoMasterPT.Value)
            {
                Assert.Greater(agentPTRow.AutoMasterPT, 0);
            }
            else
            {
                Assert.Greater(agentPTRow.MasterPT, 0);
            }

            Assert.Greater(agentPTRow.AgentPT, 0);
            Assert.GreaterOrEqual(agentPTRow.MinPT, 0);
        }

        private void AssertAgentInfo(int agentId, string agentName, IEnumerable<Customer> agentLine)
        {
            var agent = agentLine.ElementAt(2);
            Assert.AreEqual(agentId, agent.Id);
            Assert.AreEqual(agentName, agent.Name);
        }

        protected void ViewMemberPTMatrix(
            byte productId = Product255Constants.ProductId,
            int superId = SettingInfo.SuperId,
            string superName = SettingInfo.SuperUserName,
            int masterId = SettingInfo.MasterId,
            string masterName = SettingInfo.MasterUserName,
            int agentId = SettingInfo.AgentId,
            string agentName = SettingInfo.AgentUserName,
            int memberId = SettingInfo.MemberId,
            string memberName = SettingInfo.MemberUserName)
        {
            //// Act
            var memberLine = _customerService.GetCustomerLine(memberName);
            var memberPTMatrix = _positionTakingMatrixService.GetPTMatrix(productId, memberName);

            //// Assert
            //// Customer Line info
            Assert.AreEqual(4, memberLine.Count());

            AssertSuperInfo(superId, superName, memberLine);

            AssertMasterInfo(masterId, masterName, memberLine);

            AssertAgentInfo(agentId, agentName, memberLine);

            AssertMemberInfo(memberId, memberName, memberLine);

            //// PT Matrix
            Assert.IsNotEmpty(memberPTMatrix.PTItemMatrices);

            AssertSuperPTMatrixRow(memberPTMatrix);

            AssertMasterPTMatrixRow(memberPTMatrix);

            AssertAgentPTMatrixRow(memberPTMatrix);

            AssertMemberPTMatrixRow(memberPTMatrix);
        }

        private void AssertMemberPTMatrixRow(PTMatrix memberPTMatrix)
        {
            var memberPTRow = memberPTMatrix.PTItemMatrices.First().PTItemMatrixRows.ElementAt(3);

            if (memberPTRow.IsAutoSuperPT.Value)
            {
                Assert.Greater(memberPTRow.AutoSuperPT, 0);
            }
            else
            {
                Assert.Greater(memberPTRow.SuperPT, 0);
            }

            if (memberPTMatrix.IsAllowMasterAutoPT && memberPTRow.IsAutoMasterPT.Value)
            {
                Assert.Greater(memberPTRow.AutoMasterPT, 0);
            }
            else
            {
                Assert.Greater(memberPTRow.MasterPT, 0);
            }

            Assert.Greater(memberPTRow.AgentPT, 0);
            Assert.GreaterOrEqual(memberPTRow.MinPT, 0);
        }

        private void AssertMemberInfo(int memberId, string memberName, IEnumerable<Customer> memberLine)
        {
            var member = memberLine.ElementAt(3);
            Assert.AreEqual(memberId, member.Id);
            Assert.AreEqual(memberName, member.Name);
        }
    }
}