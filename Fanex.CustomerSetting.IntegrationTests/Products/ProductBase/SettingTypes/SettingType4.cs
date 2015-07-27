namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase.SettingTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using NUnit.Framework;

    public class SettingType4
    {
        public static void AssertGotSMAProductStatus(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.ProductStatusSettingTypeId).FirstOrDefault();

            var productStatus = settingType.SettingItems.ElementAt(0) as ProductStatusItem;

            Assert.IsNotNull(productStatus);
            Assert.IsNotNull(productStatus.Downline);
        }

        public static void AssertGotMemberProductStatus(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.ProductStatusSettingTypeId).FirstOrDefault();

            var productStatus = settingType.SettingItems.ElementAt(0) as ProductStatusItem;

            Assert.IsNotNull(productStatus);
        }

        public static ModifySettingTypeInfo ModifySMAProductStatus(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.ProductStatusSettingTypeId).FirstOrDefault();

            var productStatusItem = settingType.SettingItems.ElementAt(0) as ProductStatusItem;

            productStatusItem.IsDisabled = false;
            productStatusItem.Downline.IsDisabled = false;

            return new ModifySettingTypeInfo
            {
                SettingItem = productStatusItem,
                SettingTypeId = SettingTypeInfo.ProductStatusSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyMemberProductStatus(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.ProductStatusSettingTypeId).FirstOrDefault();

            var productStatusItem = settingType.SettingItems.ElementAt(0) as ProductStatusItem;

            productStatusItem.IsDisabled = false;

            return new ModifySettingTypeInfo
            {
                SettingItem = productStatusItem,
                SettingTypeId = SettingTypeInfo.ProductStatusSettingTypeId
            };
        }

        public static void AssertUpdatedSMAProductStatus(ISettingItem productStatusItem, ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.ProductStatusSettingTypeId).FirstOrDefault();

            var newProductStatus = settingType.SettingItems.ElementAt(0) as ProductStatusItem;

            var expectedProductStatus = productStatusItem as ProductStatusItem;

            Assert.AreEqual(expectedProductStatus.IsDisabled, newProductStatus.IsDisabled);
            Assert.AreEqual(expectedProductStatus.Downline.IsDisabled, newProductStatus.Downline.IsDisabled);
        }

        public static void AssertUpdatedMemberProductStatus(ISettingItem productStatusItem, ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.ProductStatusSettingTypeId).FirstOrDefault();

            var newProductStatus = settingType.SettingItems.ElementAt(0) as ProductStatusItem;

            var expectedProductStatus = productStatusItem as ProductStatusItem;

            Assert.AreEqual(expectedProductStatus.IsDisabled, newProductStatus.IsDisabled);
        }
    }
}