namespace Fanex.CustomerSetting.IntegrationTests.Products.Product255
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using Fanex.CustomerSetting.Models;
    using NUnit.Framework;
    using SettingTypesIntegration = Fanex.CustomerSetting.IntegrationTests.Products.ProductBase.SettingTypes;

    [TestFixture]
    public class Product255_AgentEditsMember_IntegrationTests : AgentEditsMember_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType1.AssertGotAgentPTOnMember,
                    SettingTypesIntegration.SettingType2.AssertGotBetLimit,
                    SettingTypesIntegration.SettingType3.AssertGotCommission,
                    SettingTypesIntegration.SettingType4.AssertGotMemberProductStatus
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType1.ModifyAgentPTOnMember,
                    SettingTypesIntegration.SettingType2.ModifyMemberBetLimit,
                    SettingTypesIntegration.SettingType3.ModifyMemberCommission,
                    SettingTypesIntegration.SettingType4.ModifyMemberProductStatus
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { ProductInfo.PTSettingTypeId, SettingTypesIntegration.SettingType1.AssertUpdatedAgentPTOnMember },
                    { ProductInfo.BetLimitSettingTypeId, SettingTypesIntegration.SettingType2.AssertUpdatedBetLimit },
                    { ProductInfo.CommissionSettingTypeId, SettingTypesIntegration.SettingType3.AssertUpdatedCommission },
                    { ProductInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedMemberProductStatus },
                };

        [Test]
        public void Product255_AgentViewsMemberSetting()
        {
            AgentViewsMemberSetting(
                _assertGotSettingTypes,
                SettingInfo.AgentId, 
                Level.Agent);
        }

        [Test]
        public void Product255_CompanyViewsMemberSettingByAgentView()
        {
            AgentViewsMemberSetting(
                _assertGotSettingTypes,
                SettingInfo.CompanyId, 
                Level.Company);
        }

        [Test]
        public void Product255_AgentUpdatesMemberSetting()
        {
            AgentUpdatesMemberSetting(_modifySettingTypes, _assertUpdatedSettingTypes);
        }

        [Test]
        public void Product255_CompanyUpdatesMemberSettingByAgentView()
        {
            CompanyUpdatesMemberSettingByAgentView(_modifySettingTypes, _assertUpdatedSettingTypes);
        }

        [Test]
        public void Product255_AgentUpdatesMultipleMembersSetting()
        {
            AgentUpdatesMultipleMembersSetting(_modifySettingTypes, _assertUpdatedSettingTypes);
        }
    }
}