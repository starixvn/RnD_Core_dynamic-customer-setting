namespace Fanex.CustomerSetting.IntegrationTests.Products.Product249
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using NUnit.Framework;
    using SettingTypesIntegration = Fanex.CustomerSetting.IntegrationTests.Products.ProductBase.SettingTypes;

    [TestFixture]
    public class Product249_AgentEditsMember_IntegrationTests : AgentEditsMember_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType2.AssertGotBetLimit,
                    SettingTypesIntegration.SettingType3.AssertGotCommission,
                    SettingTypesIntegration.SettingType4.AssertGotMemberProductStatus
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType2.ModifyMemberBetLimit,
                    SettingTypesIntegration.SettingType3.ModifyMemberCommission,
                    SettingTypesIntegration.SettingType4.ModifyMemberProductStatus
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { ProductInfo.BetLimitSettingTypeId, SettingTypesIntegration.SettingType2.AssertUpdatedBetLimit },
                    { ProductInfo.CommissionSettingTypeId, SettingTypesIntegration.SettingType3.AssertUpdatedCommission },
                    { ProductInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedMemberProductStatus }
                };

        [Test]
        public void Product249_AgentViewsMemberSetting()
        {
            AgentViewsMemberSetting(
                _assertGotSettingTypes,
                SettingInfo.AgentId,
                Level.Agent,
                SettingInfo.MemberId,
                SettingInfo.MemberUserName,
                Product249Constants.ProductId,
                Product249Constants.ProductName,
                Product249Constants.NumOfSettingType);
        }

        [Test]
        public void Product249_CompanyViewsMemberSettingByAgentView()
        {
            AgentViewsMemberSetting(
                _assertGotSettingTypes,
                SettingInfo.CompanyId,
                Level.Company,
                SettingInfo.MemberId,
                SettingInfo.MemberUserName,
                Product249Constants.ProductId,
                Product249Constants.ProductName,
                Product249Constants.NumOfSettingType);
        }

        [Test]
        public void Product249_AgentUpdatesMemberSetting()
        {
            AgentUpdatesMemberSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.AgentId,
                Product249Constants.ProductId);
        }

        [Test]
        public void Product249_CompanyUpdatesMemberSettingByAgentView()
        {
            CompanyUpdatesMemberSettingByAgentView(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.CompanyId,
                Product249Constants.ProductId);
        }

        [Test]
        public void Product249_AgentUpdatesMultipleMembersSetting()
        {
            AgentUpdatesMultipleMembersSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.AgentId,
                Product249Constants.ProductId);
        }
    }
}
