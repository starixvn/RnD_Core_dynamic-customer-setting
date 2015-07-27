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
    public class Product248_MasterEditsAgent_IntegrationTests : MasterEditsAgent_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType3.AssertGotCommission,
                    SettingTypesIntegration.SettingType4.AssertGotSMAProductStatus
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType3.ModifyAgentCommission,
                    SettingTypesIntegration.SettingType4.ModifySMAProductStatus
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { SettingTypeInfo.CommissionSettingTypeId, SettingTypesIntegration.SettingType3.AssertUpdatedCommission },
                    { SettingTypeInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedSMAProductStatus }
                };

        [Test]
        public void Product248_MasterViewsAgentSetting()
        {
            ViewingAgentSetting(
                _assertGotSettingTypes,
                SettingInfo.MasterId,
                Level.Master,
                SettingInfo.AgentId,
                SettingInfo.AgentUserName,
                Product248Constants.ProductId,
                Product248Constants.NumOfSettingType);
        }

        [Test]
        public void Product248_CompanyViewsAgentSetting()
        {
            ViewingAgentSetting(
                _assertGotSettingTypes,
                SettingInfo.CompanyId,
                Level.Company,
                SettingInfo.AgentId,
                SettingInfo.AgentUserName,
                Product248Constants.ProductId,
                Product248Constants.NumOfSettingType);
        }

        [Test]
        public void Product248_MasterUpdatesAgentSetting()
        {
            MasterUpdatesAgentSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.MasterId,
                Product248Constants.ProductId);
        }

        [Test]
        public void Product248_CompanyUpdatesAgentSetting()
        {
            CompanyUpdatesAgentSetting(
                _modifySettingTypes,
                _assertUpdatedSettingTypes,
                SettingInfo.MasterId,
                Product248Constants.ProductId);
        }

        [Test]
        public void Product248_MasterUpdatesMultipleAgentsSetting()
        {
            MasterUpdatesMultipleAgentsSetting(
                _modifySettingTypes, 
                _assertUpdatedSettingTypes,
                SettingInfo.MasterId,
                Product248Constants.ProductId);
        }
    }
}
