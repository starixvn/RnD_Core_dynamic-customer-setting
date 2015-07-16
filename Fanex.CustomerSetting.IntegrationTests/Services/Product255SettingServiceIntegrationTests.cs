namespace Fanex.CustomerSetting.IntegrationTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using Fanex.Data.Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class Product255SettingServiceIntegrationTests
    {
        private const int CompanyId = 8;
        private const int SuperId = 14424987;
        private const string SuperUserName = "supertest90";
        private const int MasterId = 14425209;
        private const string MasterUserName = "supertest9001";
        private const int AgentId = 14425546;
        private const string AgentUserName = "supertest900101";
        private const int MemberId = 14425599;
        private const string MemberUserName = "supertest900101001";
        private const string Language = "en-US";
        private const string IP = "192.168.1.1";
        private const int SystemId = 1;
        private const string Domain = "www.demo.com";
        private const byte ProductId = 255;
        private const string ProductName = "Product 255";
        private const byte NumOfSettingType = 4;
        private const int NumOfSleepSecondsWaitingForReplication = 7 * 1000; //// 7 Seconds

        private IEnumerable<int> _masterIds = new List<int> { MasterId, 14763802, 14965577 };
        private IEnumerable<string> _masterNames = new List<string> { MasterUserName, "SUPERTEST9000", "supertest9001Raf" };
        private IEnumerable<int> _agentIds = new List<int> { AgentId, 14898339, 14966320 };
        private IEnumerable<string> _agentNames = new List<string> { AgentUserName, "superrafagent", "supertest9001sunRaf" };
        private IEnumerable<int> _memberIds = new List<int> { MemberId, 14569862 };
        private IEnumerable<string> _memberNames = new List<string> { AgentUserName, "supertest900101002" };

        private IUpdateSettingService _updateSettingService;
        private IQuerySettingService _querySettingService;
        private ISettingContext _settingContext;

        [SetUp]
        public void Setup()
        {
            DbSettingProviderManager.Current.Start();
        }

        #region Get Customer Setting

        [TestCase(CompanyId, 5)]
        [TestCase(MasterId, 3)]
        public void GetCustomerSetting_ViewingAgentSetting(int actorId, int actorLevelValue)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = Enumeration.FromValue<UserLevel>(actorLevelValue), Name = "Actor Name" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(AgentId, CustomerLevel.Agent, ProductId, null);

            //// Act
            var agentSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(AgentUserName, agentSetting.CustomerInfo.UserName);
            Assert.AreEqual(AgentId, agentSetting.CustomerId);
            Assert.AreEqual(CustomerLevel.Agent, agentSetting.CustomerLevel);
            Assert.AreEqual(ProductId, agentSetting.ProductId);
            Assert.AreEqual(ProductName, agentSetting.ProductName);
            Assert.AreEqual(NumOfSettingType, agentSetting.SettingTypes.Count());

            var agentPT = agentSetting.SettingTypes.ElementAt(0) as AgentPT;
            Assert.IsNotNull(agentPT);

            var agentPTItem = agentSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTItem;
            Assert.IsNotNull(agentPTItem);
            Assert.GreaterOrEqual(agentPTItem.IM2 ? agentPTItem.UM2 : agentPTItem.M2, 1);
            Assert.GreaterOrEqual(agentPTItem.A2, 1);
            Assert.GreaterOrEqual(agentPTItem.N2, 0);

            var agentBetLimitItem = agentSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.IsNotNull(agentBetLimitItem);
            Assert.GreaterOrEqual(agentBetLimitItem.MinBet, 0);
            Assert.GreaterOrEqual(agentBetLimitItem.MaxBet, 0);
            Assert.GreaterOrEqual(agentBetLimitItem.MaxPerMatch, 0);

            var masterCommissionItem = agentSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.IsNotNull(masterCommissionItem);
            Assert.GreaterOrEqual(masterCommissionItem.CommRate, 0);
            Assert.LessOrEqual(masterCommissionItem.CommRate, 0.01);
            Assert.GreaterOrEqual(masterCommissionItem.Upline.CommRate, 0);
            Assert.LessOrEqual(masterCommissionItem.Upline.CommRate, 0.01);

            var agentProductStatusItem = agentSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            Assert.IsNotNull(agentProductStatusItem);
            Assert.IsNotNull(agentProductStatusItem.Downline);
        }

        [TestCase(CompanyId, 5)]
        [TestCase(SuperId, 4)]
        public void GetCustomerSetting_SuperViewsMemberSetting(int actorId, int actorLevelValue)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = Enumeration.FromValue<UserLevel>(actorLevelValue), Name = "Actor Name" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _querySettingService = new QuerySettingService(_settingContext);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(MemberId, CustomerLevel.Member, ProductId, CustomerLevel.Super);

            //// Act
            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(MemberUserName, memberSetting.CustomerInfo.UserName);
            Assert.AreEqual(MemberId, memberSetting.CustomerId);
            Assert.AreEqual(CustomerLevel.Member, memberSetting.CustomerLevel);
            Assert.AreEqual(ProductId, memberSetting.ProductId);
            Assert.AreEqual(ProductName, memberSetting.ProductName);
            Assert.AreEqual(NumOfSettingType - 1, memberSetting.SettingTypes.Count());

            var superPTOnMember = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTOnMemberItem;
            Assert.IsNotNull(superPTOnMember);
            Assert.GreaterOrEqual(superPTOnMember.IS1 ? superPTOnMember.US1 : superPTOnMember.S1, 0);
            Assert.GreaterOrEqual(superPTOnMember.M1, 1);
            Assert.GreaterOrEqual(superPTOnMember.A1, 1);

            var memberBetLimitItem = memberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.IsNotNull(memberBetLimitItem);
            Assert.GreaterOrEqual(memberBetLimitItem.MinBet, 0);
            Assert.GreaterOrEqual(memberBetLimitItem.MaxBet, 0);
            Assert.GreaterOrEqual(memberBetLimitItem.MaxPerMatch, 0);

            var memberCommissionItem = memberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.IsNotNull(memberCommissionItem);
            Assert.GreaterOrEqual(memberCommissionItem.CommRate, 0);
            Assert.LessOrEqual(memberCommissionItem.CommRate, 0.01);
            //// Agent comm
            Assert.GreaterOrEqual(memberCommissionItem.Upline.CommRate, 0);
            Assert.LessOrEqual(memberCommissionItem.Upline.CommRate, 0.01);
            //// Master Comm
            ////Assert.GreaterOrEqual(memberCommissionItem.Upline.Upline.CommRate, 0);
            ////Assert.LessOrEqual(memberCommissionItem.Upline.Upline.CommRate, 0.01);
        }

        [TestCase(CompanyId, 5)]
        [TestCase(MasterId, 3)]
        public void GetCustomerSetting_MasterViewsMemberSetting(int actorId, int actorLevelValue)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = Enumeration.FromValue<UserLevel>(actorLevelValue), Name = "Actor Name" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(MemberId, CustomerLevel.Member, ProductId, CustomerLevel.Master);

            //// Act
            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(MemberUserName, memberSetting.CustomerInfo.UserName);
            Assert.AreEqual(MemberId, memberSetting.CustomerId);
            Assert.AreEqual(CustomerLevel.Member, memberSetting.CustomerLevel);
            Assert.AreEqual(ProductId, memberSetting.ProductId);
            Assert.AreEqual(ProductName, memberSetting.ProductName);
            Assert.AreEqual(NumOfSettingType - 1, memberSetting.SettingTypes.Count());

            var masterPTOnMember = memberSetting.SettingTypes.ElementAt(0) as MasterPTOnMember;
            Assert.IsNotNull(masterPTOnMember);

            var masterPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTOnMemberItem;
            Assert.IsNotNull(masterPTOnMemberItem);
            Assert.GreaterOrEqual(masterPTOnMemberItem.IM1 ? masterPTOnMemberItem.UM1 : masterPTOnMemberItem.M1, 0);
            Assert.GreaterOrEqual(masterPTOnMemberItem.A1, 1);

            var memberBetLimitItem = memberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.IsNotNull(memberBetLimitItem);
            Assert.GreaterOrEqual(memberBetLimitItem.MinBet, 0);
            Assert.GreaterOrEqual(memberBetLimitItem.MaxBet, 0);
            Assert.GreaterOrEqual(memberBetLimitItem.MaxPerMatch, 0);

            var memberCommissionItem = memberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.IsNotNull(memberCommissionItem);
            Assert.GreaterOrEqual(memberCommissionItem.CommRate, 0);
            Assert.LessOrEqual(memberCommissionItem.CommRate, 0.01);
            //// Agent comm
            Assert.GreaterOrEqual(memberCommissionItem.Upline.CommRate, 0);
            Assert.LessOrEqual(memberCommissionItem.Upline.CommRate, 0.01);
        }

        [TestCase(CompanyId, 5)]
        [TestCase(AgentId, 2)]
        public void GetCustomerSetting_AgentViewsMemberSetting(int actorId, int actorLevelValue)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = Enumeration.FromValue<UserLevel>(actorLevelValue), Name = "Actor Name" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(MemberId, CustomerLevel.Member, ProductId, CustomerLevel.Agent);

            //// Act
            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);

            //// Assert
            Assert.AreEqual(MemberUserName, memberSetting.CustomerInfo.UserName);
            Assert.AreEqual(MemberId, memberSetting.CustomerId);
            Assert.AreEqual(CustomerLevel.Member, memberSetting.CustomerLevel);
            Assert.AreEqual(ProductId, memberSetting.ProductId);
            Assert.AreEqual(ProductName, memberSetting.ProductName);
            Assert.AreEqual(NumOfSettingType, memberSetting.SettingTypes.Count());

            var agentPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTOnMemberItem;
            Assert.IsNotNull(agentPTOnMemberItem);
            Assert.GreaterOrEqual(agentPTOnMemberItem.A1, 1);

            var memberBetLimitItem = memberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.IsNotNull(memberBetLimitItem);
            Assert.GreaterOrEqual(memberBetLimitItem.MinBet, 0);
            Assert.GreaterOrEqual(memberBetLimitItem.MaxBet, 0);
            Assert.GreaterOrEqual(memberBetLimitItem.MaxPerMatch, 0);

            var memberCommissionItem = memberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.IsNotNull(memberCommissionItem);
            Assert.GreaterOrEqual(memberCommissionItem.CommRate, 0);
            Assert.LessOrEqual(memberCommissionItem.CommRate, 0.01);
            //// Agent comm
            Assert.GreaterOrEqual(memberCommissionItem.Upline.CommRate, 0);
            Assert.LessOrEqual(memberCommissionItem.Upline.CommRate, 0.01);
        }

        #endregion Get Customer Setting

        #region Update Single Customer Setting

        [Test]
        public void UpdateCustomerSetting_CompanyUpdatesAgentSetting()
        {
            UpdateCustomerSetting_UpdatingAgentSetting(CompanyId, UserLevel.Company, true);
            UpdateCustomerSetting_UpdatingAgentSetting(CompanyId, UserLevel.Company, false);
        }

        [Test]
        public void UpdateCustomerSetting_MasterUpdatesAgentSetting()
        {
            UpdateCustomerSetting_UpdatingAgentSetting(MasterId, UserLevel.Master, true);
            UpdateCustomerSetting_UpdatingAgentSetting(MasterId, UserLevel.Master, false);
        }

        private void UpdateCustomerSetting_UpdatingAgentSetting(int actorId, UserLevel actorLevel, bool isDisabledProductStatus)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(AgentId, CustomerLevel.Agent, ProductId, null);

            var agentSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var agentPT = agentSetting.SettingTypes.ElementAt(0) as AgentPT;
            var agentPTItem = agentSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTItem;

            if (!agentPT.IsDisableMasterAutoPT)
            {
                agentPTItem.IM2 = !agentPTItem.IM2;
            }

            if (agentPTItem.IM2)
            {
                agentPTItem.UM2 = (byte)(agentPTItem.UM2 == 20 ? 21 : 20);
                agentPTItem.M2 = 0;
            }
            else
            {
                agentPTItem.M2 = (byte)(agentPTItem.M2 == 20 ? 21 : 20);
                agentPTItem.UM2 = 0;
            }

            agentPTItem.A2 = (byte)(agentPTItem.A2 == 35 ? 36 : 35);
            agentPTItem.N2 = (byte)(agentPTItem.N2 == 5 ? 6 : 5);

            var agentBetLimitItem = agentSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            agentBetLimitItem.MinBet = agentBetLimitItem.MinBet == 60 ? 61 : 60;
            agentBetLimitItem.MaxBet = agentBetLimitItem.MaxBet == 400 ? 401 : 400;
            agentBetLimitItem.MaxPerMatch = agentBetLimitItem.MaxPerMatch == 350550 ? 350551 : 350550;

            var agentCommissionItem = agentSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            agentCommissionItem.CommRate = agentCommissionItem.CommRate == 0.0055m ? 0.0056m : 0.0055m;

            var agentProductStatusItem = agentSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            agentProductStatusItem.IsDisabled = isDisabledProductStatus;
            agentProductStatusItem.Downline.IsDisabled = isDisabledProductStatus;

            //// Act
            var updateParam = new UpdatingSettingParam(agentSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.
            var newAgentSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var newAgentPTItem = newAgentSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTItem;
            Assert.AreEqual(agentPTItem.IM2, newAgentPTItem.IM2);
            Assert.AreEqual(agentPTItem.UM2, newAgentPTItem.UM2);
            Assert.AreEqual(agentPTItem.M2, newAgentPTItem.M2);
            Assert.AreEqual(agentPTItem.A2, newAgentPTItem.A2);
            Assert.AreEqual(agentPTItem.N2, newAgentPTItem.N2);

            var newAgentBetLimitItem = newAgentSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.AreEqual(agentBetLimitItem.MinBet, newAgentBetLimitItem.MinBet);
            Assert.AreEqual(agentBetLimitItem.MaxBet, newAgentBetLimitItem.MaxBet);
            Assert.AreEqual(agentBetLimitItem.MaxPerMatch, newAgentBetLimitItem.MaxPerMatch);

            var newAgentCommissionItem = newAgentSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.AreEqual(agentCommissionItem.CommRate, newAgentCommissionItem.CommRate);

            var newAgentProductStatusItem = newAgentSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            Assert.AreEqual(agentProductStatusItem.IsDisabled, newAgentProductStatusItem.IsDisabled);
            Assert.AreEqual(agentProductStatusItem.Downline.IsDisabled, newAgentProductStatusItem.Downline.IsDisabled);
        }

        [Test]
        public void UpdateCustomerSetting_AgentUpdatesMemberSetting()
        {
            UpdateCustomerSetting_AgentUpdatesMemberSetting(AgentId, UserLevel.Agent, true);
            UpdateCustomerSetting_AgentUpdatesMemberSetting(AgentId, UserLevel.Agent, false);
        }

        [Test]
        public void UpdateCustomerSetting_CompanyUpdatesMemberSettingByAgentView()
        {
            UpdateCustomerSetting_AgentUpdatesMemberSetting(CompanyId, UserLevel.Company, true);
            UpdateCustomerSetting_AgentUpdatesMemberSetting(CompanyId, UserLevel.Company, false);
        }

        private void UpdateCustomerSetting_AgentUpdatesMemberSetting(int actorId, UserLevel actorLevel, bool isDisabledProductStatus)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(MemberId, CustomerLevel.Member, ProductId, CustomerLevel.Agent);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var agentPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTOnMemberItem;
            agentPTOnMemberItem.A1 = (byte)(agentPTOnMemberItem.A2 == 14 ? 15 : 14);

            var memberBetLimitItem = memberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            memberBetLimitItem.MinBet = memberBetLimitItem.MinBet == 70 ? 71 : 70;
            memberBetLimitItem.MaxBet = memberBetLimitItem.MaxBet == 300 ? 301 : 300;
            memberBetLimitItem.MaxPerMatch = memberBetLimitItem.MaxPerMatch == 250550 ? 250551 : 250550;

            var memberCommissionItem = memberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            memberCommissionItem.CommRate = memberCommissionItem.CommRate == 0.0033m ? 0.0032m : 0.0033m;

            var memberProductStatusItem = memberSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            memberProductStatusItem.IsDisabled = isDisabledProductStatus;

            //// Act
            var updateParam = new UpdatingSettingParam(memberSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.
            var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var newAgentOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTOnMemberItem;
            Assert.AreEqual(agentPTOnMemberItem.A1, newAgentOnMemberPTItem.A1);

            var newMemberBetLimitItem = newMemberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            Assert.AreEqual(memberBetLimitItem.MinBet, newMemberBetLimitItem.MinBet);
            Assert.AreEqual(memberBetLimitItem.MaxBet, newMemberBetLimitItem.MaxBet);
            Assert.AreEqual(memberBetLimitItem.MaxPerMatch, newMemberBetLimitItem.MaxPerMatch);

            var newMemberCommissionItem = newMemberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            Assert.AreEqual(memberCommissionItem.CommRate, newMemberCommissionItem.CommRate);

            var newMemberProductStatusItem = newMemberSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            Assert.AreEqual(memberProductStatusItem.IsDisabled, newMemberProductStatusItem.IsDisabled);
        }

        [Test]
        public void UpdateCustomerSetting_MasterUpdatesPTOnMember()
        {
            //// Update two time for auto pt and not auto pt
            UpdateCustomerSetting_MasterUpdatesPTOnMember(MasterId, UserLevel.Master);
            UpdateCustomerSetting_MasterUpdatesPTOnMember(MasterId, UserLevel.Master);
        }

        [Test]
        public void UpdateCustomerSetting_CompanyUpdatesMasterPTOnMember()
        {
            //// Update two time for auto pt and not auto pt
            UpdateCustomerSetting_MasterUpdatesPTOnMember(CompanyId, UserLevel.Company);
            UpdateCustomerSetting_MasterUpdatesPTOnMember(CompanyId, UserLevel.Company);
        }

        private void UpdateCustomerSetting_MasterUpdatesPTOnMember(int actorId, UserLevel actorLevel)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(MemberId, CustomerLevel.Member, ProductId, CustomerLevel.Master);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var masterPTOnMember = memberSetting.SettingTypes.ElementAt(0) as MasterPTOnMember;
            var masterPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTOnMemberItem;
            if (!masterPTOnMember.IsDisableMasterAutoPT)
            {
                masterPTOnMemberItem.IM1 = !masterPTOnMemberItem.IM1;
            }

            if (masterPTOnMemberItem.IM1)
            {
                masterPTOnMemberItem.UM1 = (byte)(masterPTOnMemberItem.UM1 == 14 ? 15 : 14);
                masterPTOnMemberItem.M1 = 0;
            }
            else
            {
                masterPTOnMemberItem.M1 = (byte)(masterPTOnMemberItem.M1 == 14 ? 15 : 14);
                masterPTOnMemberItem.UM1 = 0;
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updateParam = new UpdatingSettingParam(memberSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.
            var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var newMasterOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTOnMemberItem;
            Assert.AreEqual(masterPTOnMemberItem.M1, newMasterOnMemberPTItem.M1);
            Assert.AreEqual(masterPTOnMemberItem.UM1, newMasterOnMemberPTItem.UM1);
        }

        [Test]
        public void UpdateCustomerSetting_SuperUpdatesPTOnMember()
        {
            //// Waiting for anh Johnson fixes bug
            //// Update two time for auto pt and not auto pt
            ////UpdateCustomerSetting_SuperUpdatesPTOnMember(SuperId, UserLevel.Super);
            ////UpdateCustomerSetting_SuperUpdatesPTOnMember(SuperId, UserLevel.Super);
        }

        [Test]
        public void UpdateCustomerSetting_CompanyUpdatesSuperPTOnMember()
        {
            //// Waiting for anh Johnson fixes bug
            ///// Update two time for auto pt and not auto pt
            ////UpdateCustomerSetting_SuperUpdatesPTOnMember(CompanyId, UserLevel.Company);
            ////UpdateCustomerSetting_SuperUpdatesPTOnMember(CompanyId, UserLevel.Company);
        }

        private void UpdateCustomerSetting_SuperUpdatesPTOnMember(int actorId, UserLevel actorLevel)
        {
            //// Arrange
            var actor = new User { Id = actorId, Level = actorLevel, Name = "ActorName" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _updateSettingService = new UpdateSettingService(_settingContext);

            var querySettingParam = new QuerySettingParam(MemberId, CustomerLevel.Member, ProductId, CustomerLevel.Super);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var superPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTOnMemberItem;
            superPTOnMemberItem.IS1 = !superPTOnMemberItem.IS1;

            if (superPTOnMemberItem.IS1)
            {
                superPTOnMemberItem.US1 = (byte)(superPTOnMemberItem.US1 == 15 ? 16 : 15);
                superPTOnMemberItem.S1 = 0;
            }
            else
            {
                superPTOnMemberItem.S1 = (byte)(superPTOnMemberItem.S1 == 15 ? 16 : 15);
                superPTOnMemberItem.US1 = 0;
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updateParam = new UpdatingSettingParam(memberSetting, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateCustomerSetting(updateParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(NumOfSleepSecondsWaitingForReplication); //// Wait for replication data.

            var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var newSuperOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as SuperPTOnMemberItem;
            Assert.AreEqual(superPTOnMemberItem.S1, newSuperOnMemberPTItem.S1);
            Assert.AreEqual(superPTOnMemberItem.US1, newSuperOnMemberPTItem.US1);
        }

        #endregion Update Single Customer Setting

        #region Update Multiple Customes Setting

        [Test]
        public void UpdateMultipleCustomerSetting_MasterUpdatesMultipleAgentsSetting()
        {
            //// Arrange
            var actor = new User { Id = MasterId, Level = UserLevel.Master, Name = "ActorName" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(AgentId, CustomerLevel.Agent, ProductId, null);

            var agentSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var agentPT = agentSetting.SettingTypes.ElementAt(0) as AgentPT;
            var agentPTItem = agentSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTItem;
            if (!agentPT.IsDisableMasterAutoPT && agentPTItem.IM2)
            {
                agentPTItem.UM2 = (byte)(agentPTItem.UM2 == 20 ? 21 : 20);
            }
            else
            {
                agentPTItem.M2 = (byte)(agentPTItem.M2 == 20 ? 21 : 20);
            }

            agentPTItem.A2 = (byte)(agentPTItem.A2 == 35 ? 36 : 35);
            agentPTItem.N2 = (byte)(agentPTItem.N2 == 5 ? 6 : 5);

            var agentBetLimitItem = agentSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            agentBetLimitItem.MinBet = agentBetLimitItem.MinBet == 60 ? 61 : 60;
            agentBetLimitItem.MaxBet = agentBetLimitItem.MaxBet == 400 ? 401 : 400;
            agentBetLimitItem.MaxPerMatch = agentBetLimitItem.MaxPerMatch == 350550 ? 350551 : 350550;

            var agentCommissionItem = agentSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            agentCommissionItem.CommRate = agentCommissionItem.CommRate == 0.0055m ? 0.0056m : 0.0055m;

            var agentProductStatusItem = agentSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            agentProductStatusItem.IsDisabled = false;
            agentProductStatusItem.Downline.IsDisabled = false;

            //// Act
            var updatingMultipleParam = new UpdatingMultipleSettingParam(agentSetting, _agentIds, _agentNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateMultipleCustomerSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(NumOfSleepSecondsWaitingForReplication * 2); //// Wait for replication data.

            foreach (var agentId in _agentIds)
            {
                querySettingParam.CustomerId = agentId;
                var newAgentSetting = _querySettingService.GetCustomerSetting(querySettingParam);
                var newAgentPTItem = newAgentSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTItem;
                Assert.AreEqual(agentPTItem.IM2, newAgentPTItem.IM2);
                Assert.AreEqual(agentPTItem.UM2, newAgentPTItem.UM2);
                Assert.AreEqual(agentPTItem.M2, newAgentPTItem.M2);
                Assert.AreEqual(agentPTItem.A2, newAgentPTItem.A2);
                Assert.AreEqual(agentPTItem.N2, newAgentPTItem.N2);

                var newAgentBetLimitItem = newAgentSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
                Assert.AreEqual(agentBetLimitItem.MinBet, newAgentBetLimitItem.MinBet);
                Assert.AreEqual(agentBetLimitItem.MaxBet, newAgentBetLimitItem.MaxBet);
                Assert.AreEqual(agentBetLimitItem.MaxPerMatch, newAgentBetLimitItem.MaxPerMatch);

                var newAgentCommissionItem = newAgentSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
                Assert.AreEqual(agentCommissionItem.CommRate, newAgentCommissionItem.CommRate);

                var newAgentProductStatusItem = newAgentSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
                Assert.AreEqual(agentProductStatusItem.IsDisabled, newAgentProductStatusItem.IsDisabled);
                Assert.AreEqual(agentProductStatusItem.Downline.IsDisabled, newAgentProductStatusItem.Downline.IsDisabled);
            }
        }

        [Test]
        public void UpdateMultipleCustomerSetting_AgentUpdatesMultipleMembersSetting()
        {
            //// Arrange
            var actor = new User { Id = AgentId, Level = UserLevel.Agent, Name = "ActorName" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _updateSettingService = new UpdateSettingService(_settingContext);

            var querySettingParam = new QuerySettingParam(MemberId, CustomerLevel.Member, ProductId, CustomerLevel.Agent);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var agentPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTOnMemberItem;
            agentPTOnMemberItem.A1 = (byte)(agentPTOnMemberItem.A2 == 14 ? 15 : 14);

            var memberBetLimitItem = memberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
            memberBetLimitItem.MinBet = memberBetLimitItem.MinBet == 70 ? 71 : 70;
            memberBetLimitItem.MaxBet = memberBetLimitItem.MaxBet == 300 ? 301 : 300;
            memberBetLimitItem.MaxPerMatch = memberBetLimitItem.MaxPerMatch == 250550 ? 250551 : 250550;

            var memberCommissionItem = memberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
            memberCommissionItem.CommRate = memberCommissionItem.CommRate == 0.0033m ? 0.0032m : 0.0033m;

            var memberProductStatusItem = memberSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
            memberProductStatusItem.IsDisabled = false;

            //// Act
            var updatingMultipleParam = new UpdatingMultipleSettingParam(memberSetting, _memberIds, _memberNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateMultipleCustomerSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(NumOfSleepSecondsWaitingForReplication * 2); //// Wait for replication data.

            foreach (var memberId in _memberIds)
            {
                querySettingParam.CustomerId = memberId;
                var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
                var newAgentOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as AgentPTOnMemberItem;
                Assert.AreEqual(agentPTOnMemberItem.A1, newAgentOnMemberPTItem.A1);

                var newMemberBetLimitItem = newMemberSetting.SettingTypes.ElementAt(1).SettingItems.ElementAt(0) as BetLimitItem;
                Assert.AreEqual(memberBetLimitItem.MinBet, newMemberBetLimitItem.MinBet);
                Assert.AreEqual(memberBetLimitItem.MaxBet, newMemberBetLimitItem.MaxBet);
                Assert.AreEqual(memberBetLimitItem.MaxPerMatch, newMemberBetLimitItem.MaxPerMatch);

                var newMemberCommissionItem = newMemberSetting.SettingTypes.ElementAt(2).SettingItems.ElementAt(0) as CommissionItem;
                Assert.AreEqual(memberCommissionItem.CommRate, newMemberCommissionItem.CommRate);

                var newMemberProductStatusItem = newMemberSetting.SettingTypes.ElementAt(3).SettingItems.ElementAt(0) as ProductStatusItem;
                Assert.AreEqual(memberProductStatusItem.IsDisabled, newMemberProductStatusItem.IsDisabled);
            }
        }

        [Test]
        public void UpdateMultipleCustomerSetting_MasterUpdatesPTOnMultipleMembesr()
        {
            //// Arrange
            var actor = new User { Id = MasterId, Level = UserLevel.Master, Name = "ActorName" };
            _settingContext = new SettingContext(actor, Language, IP, SystemId, Domain);
            _updateSettingService = new UpdateSettingService(_settingContext);
            var querySettingParam = new QuerySettingParam(MemberId, CustomerLevel.Member, ProductId, CustomerLevel.Master);

            var memberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
            var masterPTOnMember = memberSetting.SettingTypes.ElementAt(0) as MasterPTOnMember;
            var masterPTOnMemberItem = memberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTOnMemberItem;
            if (!masterPTOnMember.IsDisableMasterAutoPT && masterPTOnMemberItem.IM1)
            {
                masterPTOnMemberItem.UM1 = (byte)(masterPTOnMemberItem.UM1 == 14 ? 15 : 14);
            }
            else
            {
                masterPTOnMemberItem.M1 = (byte)(masterPTOnMemberItem.M1 == 14 ? 15 : 14);
            }

            memberSetting.SettingTypes = memberSetting.SettingTypes.Take(1);

            //// Act
            var updatingMultipleParam = new UpdatingMultipleSettingParam(memberSetting, _memberIds, _memberNames, querySettingParam.UplineLevel);
            var updateResult = _updateSettingService.UpdateMultipleCustomerSetting(updatingMultipleParam);

            //// Assert
            Assert.IsTrue(updateResult.IsSuccessful);

            Thread.Sleep(NumOfSleepSecondsWaitingForReplication * 2); //// Wait for replication data.

            foreach (var memberId in _memberIds)
            {
                querySettingParam.CustomerId = memberId;
                var newMemberSetting = _querySettingService.GetCustomerSetting(querySettingParam);
                var newMasterOnMemberPTItem = newMemberSetting.SettingTypes.ElementAt(0).SettingItems.ElementAt(0) as MasterPTOnMemberItem;
                Assert.AreEqual(masterPTOnMemberItem.M1, newMasterOnMemberPTItem.M1);
                Assert.AreEqual(masterPTOnMemberItem.UM1, newMasterOnMemberPTItem.UM1);
            }
        }

        #endregion Update Multiple Customes Setting
    }
}