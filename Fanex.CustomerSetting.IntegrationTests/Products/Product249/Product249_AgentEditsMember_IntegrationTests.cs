namespace Fanex.CustomerSetting.IntegrationTests.Products.Product249
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.IntegrationTests.Products.ProductBase;
    using Fanex.CustomerSetting.Models;
    using NUnit.Framework;
    using SettingTypesIntegration = Fanex.CustomerSetting.IntegrationTests.Products.ProductBase.SettingTypes;

    public class Product249_AgentEditsMember_IntegrationTests : AgentEditsMember_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType2.AssertGotBetLimit,
                    SettingTypesIntegration.SettingType4.AssertGotMemberProductStatus
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType2.ModifyMemberBetLimit,
                    SettingTypesIntegration.SettingType4.ModifyMemberProductStatus
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { ProductInfo.BetLimitSettingTypeId, SettingTypesIntegration.SettingType2.AssertUpdatedBetLimit },
                    { ProductInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedMemberProductStatus }
                };

        [Test]
        public void Product255_AgentViewsMemberSetting()
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
        public void Product255_CompanyViewsMemberSettingByAgentView()
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
        public void Product255_AgentUpdatesMemberSetting()
        {
            AgentUpdatesMemberSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.AgentId,
                Product249Constants.ProductId);
        }

        [Test]
        public void Product255_CompanyUpdatesMemberSettingByAgentView()
        {
            CompanyUpdatesMemberSettingByAgentView(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.CompanyId,
                Product249Constants.ProductId);
        }

        [Test]
        public void Product255_AgentUpdatesMultipleMembersSetting()
        {
            AgentUpdatesMultipleMembersSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.AgentId,
                Product249Constants.ProductId);
        }
    }
}
