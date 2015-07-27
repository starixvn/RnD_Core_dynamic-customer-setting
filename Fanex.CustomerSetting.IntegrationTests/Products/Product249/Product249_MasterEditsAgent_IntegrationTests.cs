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
    public class Product249_MasterEditsAgent_IntegrationTests : MasterEditsAgent_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType2.AssertGotBetLimit,
                    SettingTypesIntegration.SettingType3.AssertGotCommission,
                    SettingTypesIntegration.SettingType4.AssertGotSMAProductStatus
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType2.ModifyAgentBetLimit,
                    SettingTypesIntegration.SettingType3.ModifyAgentCommission,
                    SettingTypesIntegration.SettingType4.ModifySMAProductStatus
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { ProductInfo.BetLimitSettingTypeId, SettingTypesIntegration.SettingType2.AssertUpdatedBetLimit },
                    { ProductInfo.CommissionSettingTypeId, SettingTypesIntegration.SettingType3.AssertUpdatedCommission },
                    { ProductInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedSMAProductStatus }
                };

        [Test]
        public void Product249_MasterViewsAgentSetting()
        {
            ViewingAgentSetting(
                _assertGotSettingTypes,
                SettingInfo.MasterId,
                Level.Master,
                SettingInfo.AgentId,
                SettingInfo.AgentUserName,
                Product249Constants.ProductId,
                Product249Constants.ProductName,
                Product249Constants.NumOfSettingType);
        }

        [Test]
        public void Product249_CompanyViewsAgentSetting()
        {
            ViewingAgentSetting(
                _assertGotSettingTypes,
                SettingInfo.CompanyId,
                Level.Company,
                SettingInfo.AgentId,
                SettingInfo.AgentUserName,
                Product249Constants.ProductId,
                Product249Constants.ProductName,
                Product249Constants.NumOfSettingType);
        }

        [Test]
        public void Product249_MasterUpdatesAgentSetting()
        {
            MasterUpdatesAgentSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.MasterId,
                Product249Constants.ProductId);
        }

        [Test]
        public void Product249_CompanyUpdatesAgentSetting()
        {
            CompanyUpdatesAgentSetting(
                _modifySettingTypes,
                _assertUpdatedSettingTypes,
                SettingInfo.MasterId,
                Product249Constants.ProductId);
        }

        [Test]
        public void Product249_MasterUpdatesMultipleAgentsSetting()
        {
            MasterUpdatesMultipleAgentsSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.MasterId,
                Product249Constants.ProductId);
        }
    }
}
