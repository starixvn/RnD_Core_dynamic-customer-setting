namespace Fanex.CustomerSetting.IntegrationTests.Products.Product255
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
    public class Product255_MasterEditsAgent_IntegrationTests : MasterEditsAgent_IntegrationTests
    {
        private IEnumerable<Action<ISetting>> _assertGotSettingTypes = new List<Action<ISetting>>
                {
                    SettingTypesIntegration.SettingType1.AssertGotAgentPT,
                    SettingTypesIntegration.SettingType2.AssertGotBetLimit,
                    SettingTypesIntegration.SettingType3.AssertGotCommission,
                    SettingTypesIntegration.SettingType4.AssertGotSMAProductStatus
                };

        private IEnumerable<Func<ISetting, ModifySettingTypeInfo>> _modifySettingTypes = new List<Func<ISetting, ModifySettingTypeInfo>>
                {
                    SettingTypesIntegration.SettingType1.ModifyAgentPT,
                    SettingTypesIntegration.SettingType2.ModifyAgentBetLimit,
                    SettingTypesIntegration.SettingType3.ModifyAgentCommission,
                    SettingTypesIntegration.SettingType4.ModifySMAProductStatus
                };

        private Dictionary<byte, Action<ISettingItem, ISetting>> _assertUpdatedSettingTypes =
                new Dictionary<byte, Action<ISettingItem, ISetting>>
                {
                    { ProductInfo.PTSettingTypeId, SettingTypesIntegration.SettingType1.AssertUpdatedAgentPT },
                    { ProductInfo.BetLimitSettingTypeId, SettingTypesIntegration.SettingType2.AssertUpdatedBetLimit },
                    { ProductInfo.CommissionSettingTypeId, SettingTypesIntegration.SettingType3.AssertUpdatedCommission },
                    { ProductInfo.ProductStatusSettingTypeId, SettingTypesIntegration.SettingType4.AssertUpdatedSMAProductStatus },
                };

        [Test]
        public void Product255_MasterViewsAgentSetting()
        {
            ViewingAgentSetting(
                _assertGotSettingTypes,
                SettingInfo.MasterId, 
                Level.Master);
        }

        [Test]
        public void Product255_CompanyViewsAgentSetting()
        {
            ViewingAgentSetting(
                _assertGotSettingTypes,
                SettingInfo.CompanyId, 
                Level.Company);
        }

        [Test]
        public void Product255_MasterUpdatesAgentSetting()
        {
            MasterUpdatesAgentSetting(_modifySettingTypes, _assertUpdatedSettingTypes);
        }

        [Test]
        public void Product255_CompanyUpdatesAgentSetting()
        {
            CompanyUpdatesAgentSetting(_modifySettingTypes, _assertUpdatedSettingTypes);
        }

        [Test]
        public void Product255_MasterUpdatesMultipleAgentsSetting()
        {
            MasterUpdatesMultipleAgentsSetting(_modifySettingTypes, _assertUpdatedSettingTypes);
        }
    }
}