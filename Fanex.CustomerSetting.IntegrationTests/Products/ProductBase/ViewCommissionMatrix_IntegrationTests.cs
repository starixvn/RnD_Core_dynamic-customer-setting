namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase
{
    using System.Linq;
    using Fanex.CustomerSetting.IntegrationTests.Products.Product255;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Services;
    using Fanex.CustomerSetting.Matrix.Services.Impl;
    using Fanex.Data.Configuration;
    using NUnit.Framework;

    public class ViewCommissionMatrix_IntegrationTests
    {
        private ICommissionMatrixService _commissionMatrixService;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
            _commissionMatrixService = new CommissionMatrixService();
        }

        protected void ViewSuperCommissionMatrix(
            byte productId = Product255Constants.ProductId,
            int superId = SettingInfo.SuperId,
            string superName = SettingInfo.SuperUserName)
        {
            //// Act
            var superCommissionMatrix = _commissionMatrixService.GetCommissionMatrix(productId, superName);

            //// Commission Matrix
            Assert.IsNotEmpty(superCommissionMatrix.CommissionItemMatrices);
            Assert.Greater(superCommissionMatrix.SettingLevel, 0);
            Assert.AreEqual(1, superCommissionMatrix.CommissionItemMatrices.First().CommissionItemMatrixRows.Count());

            AssertSuperCommissionMatrixRow(superCommissionMatrix);
        }

        private void AssertSuperCommissionMatrixRow(CommissionMatrix commissionMatrix)
        {
            var superCommissionRow = commissionMatrix.CommissionItemMatrices.First().CommissionItemMatrixRows.First();
            Assert.GreaterOrEqual(superCommissionRow.CommRate, 0);
            Assert.LessOrEqual(superCommissionRow.CommRate, 0.01);
            Assert.IsTrue(superCommissionRow.CustomerLevel.IsSuperLevel);
        }

        protected void ViewMasterCommissionMatrix(
            byte productId = Product255Constants.ProductId,
            int superId = SettingInfo.SuperId,
            string superName = SettingInfo.SuperUserName,
            int masterId = SettingInfo.MasterId,
            string masterName = SettingInfo.MasterUserName)
        {
            //// Act
            var masterCommissionMatrix = _commissionMatrixService.GetCommissionMatrix(productId, masterName);

            //// Assert

            Assert.IsNotEmpty(masterCommissionMatrix.CommissionItemMatrices);

            AssertSuperCommissionMatrixRow(masterCommissionMatrix);

            AssertMasterCommissionMatrixRow(masterCommissionMatrix);
        }

        private void AssertMasterCommissionMatrixRow(CommissionMatrix masterCommissionMatrix)
        {
            var masterCommissionRow = masterCommissionMatrix.CommissionItemMatrices.First().CommissionItemMatrixRows.ElementAt(1);

            Assert.IsTrue(masterCommissionRow.CustomerLevel.IsMasterLevel);
            Assert.GreaterOrEqual(masterCommissionRow.CommRate, 0);
            Assert.LessOrEqual(masterCommissionRow.CommRate, 0.01);
        }

        protected void ViewAgentCommissionMatrix(
            byte productId = Product255Constants.ProductId,
            int superId = SettingInfo.SuperId,
            string superName = SettingInfo.SuperUserName,
            int masterId = SettingInfo.MasterId,
            string masterName = SettingInfo.MasterUserName,
            int agentId = SettingInfo.AgentId,
            string agentName = SettingInfo.AgentUserName)
        {
            //// Act
            var agentCommissionMatrix = _commissionMatrixService.GetCommissionMatrix(productId, agentName);

            //// Assert
            Assert.IsNotEmpty(agentCommissionMatrix.CommissionItemMatrices);

            AssertSuperCommissionMatrixRow(agentCommissionMatrix);

            AssertMasterCommissionMatrixRow(agentCommissionMatrix);

            AssertAgentCommissionMatrixRow(agentCommissionMatrix);
        }

        private void AssertAgentCommissionMatrixRow(CommissionMatrix agentCommissionMatrix)
        {
            var agentCommissionRow = agentCommissionMatrix.CommissionItemMatrices.First().CommissionItemMatrixRows.ElementAt(2);

            Assert.IsTrue(agentCommissionRow.CustomerLevel.IsAgentLevel);
            Assert.GreaterOrEqual(agentCommissionRow.CommRate, 0);
            Assert.LessOrEqual(agentCommissionRow.CommRate, 0.01);
        }

        protected void ViewMemberCommissionMatrix(
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
            var memberCommissionMatrix = _commissionMatrixService.GetCommissionMatrix(productId, memberName);

            //// Assert
            Assert.IsNotEmpty(memberCommissionMatrix.CommissionItemMatrices);

            AssertSuperCommissionMatrixRow(memberCommissionMatrix);

            AssertMasterCommissionMatrixRow(memberCommissionMatrix);

            AssertAgentCommissionMatrixRow(memberCommissionMatrix);

            AssertMemberCommissionMatrixRow(memberCommissionMatrix);
        }

        private void AssertMemberCommissionMatrixRow(CommissionMatrix memberCommissionMatrix)
        {
            var memberCommissionRow = memberCommissionMatrix.CommissionItemMatrices.First().CommissionItemMatrixRows.ElementAt(3);

            Assert.IsTrue(memberCommissionRow.CustomerLevel.IsMemberLevel);
            Assert.GreaterOrEqual(memberCommissionRow.CommRate, 0);
            Assert.LessOrEqual(memberCommissionRow.CommRate, 0.01);
        }
    }
}