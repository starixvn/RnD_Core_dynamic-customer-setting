namespace Fanex.CustomerSetting.IntegrationTests.Products.Product248
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
    public class Product248_AgentEditsMember_IntegrationTests : AgentEditsMember_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType3.AssertGotCommission,
                    SettingTypesIntegration.SettingType4.AssertGotMemberProductStatus
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType3.ModifyMemberCommission,
                    SettingTypesIntegration.SettingType4.ModifyMemberProductStatus
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { SettingTypeInfo.CommissionSettingTypeId, SettingTypesIntegration.SettingType3.AssertUpdatedCommission },
                    { SettingTypeInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedMemberProductStatus }
                };

        [Test]
        public void Product248_AgentViewsMemberSetting()
        {
            AgentViewsMemberSetting(
                _assertGotSettingTypes,
                SettingInfo.AgentId,
                Level.Agent,
                SettingInfo.MemberId,
                SettingInfo.MemberUserName,
                Product248Constants.ProductId,
                Product248Constants.NumOfSettingType);
        }

        [Test]
        public void Product248_CompanyViewsMemberSettingByAgentView()
        {
            AgentViewsMemberSetting(
                _assertGotSettingTypes,
                SettingInfo.CompanyId,
                Level.Company,
                SettingInfo.MemberId,
                SettingInfo.MemberUserName,
                Product248Constants.ProductId,
                Product248Constants.NumOfSettingType);
        }

        [Test]
        public void Product248_AgentUpdatesMemberSetting()
        {
            AgentUpdatesMemberSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.AgentId,
                Product248Constants.ProductId);
        }

        [Test]
        public void Product248_CompanyUpdatesMemberSettingByAgentView()
        {
            CompanyUpdatesMemberSettingByAgentView(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.CompanyId,
                Product248Constants.ProductId);
        }

        [Test]
        public void Product248_AgentUpdatesMultipleMembersSetting()
        {
            AgentUpdatesMultipleMembersSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.AgentId,
                Product248Constants.ProductId);
        }
    }
}
