namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase.SettingTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using NUnit.Framework;

    public static class SettingType1
    {
        #region Assert Got PT

        public static void AssertGotSuperPT(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var superPTITem = settingType.SettingItems.ElementAt(0) as SuperPTItem;

            Assert.IsNotNull(superPTITem);
            Assert.GreaterOrEqual(superPTITem.A4, 1);
            Assert.GreaterOrEqual(superPTITem.DefaultMaxPT, 1);
            Assert.GreaterOrEqual(superPTITem.M4, 1);
            Assert.GreaterOrEqual(superPTITem.S4, 1);
            Assert.GreaterOrEqual(superPTITem.N4, 0);
        }

        public static void AssertGotMasterPT(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var masterPTItem = settingType.SettingItems.ElementAt(0) as MasterPTItem;

            Assert.IsNotNull(masterPTItem);
            Assert.GreaterOrEqual(masterPTItem.IS3 ? masterPTItem.US3 : masterPTItem.S3, 1);
            Assert.GreaterOrEqual(masterPTItem.M3, 1);
            Assert.GreaterOrEqual(masterPTItem.N3, 0);
        }

        public static void AssertGotAgentPT(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var agentPTItem = settingType.SettingItems.ElementAt(0) as AgentPTItem;

            Assert.IsNotNull(agentPTItem);
            Assert.GreaterOrEqual(agentPTItem.IM2 ? agentPTItem.UM2 : agentPTItem.M2, 1);
            Assert.GreaterOrEqual(agentPTItem.A2, 1);
            Assert.GreaterOrEqual(agentPTItem.N2, 0);
        }

        public static void AssertGotAgentPTOnMember(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var agentPTOnMemberItem = settingType.SettingItems.ElementAt(0) as AgentPTOnMemberItem;

            Assert.IsNotNull(agentPTOnMemberItem);
            Assert.GreaterOrEqual(agentPTOnMemberItem.A1, 1);
        }

        public static void AssertGotMasterPTOnMember(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var masterPTOnMemberItem = settingType.SettingItems.ElementAt(0) as MasterPTOnMemberItem;

            Assert.IsNotNull(masterPTOnMemberItem);
            Assert.GreaterOrEqual(masterPTOnMemberItem.IM1 ? masterPTOnMemberItem.UM1 : masterPTOnMemberItem.M1, 0);
            Assert.GreaterOrEqual(masterPTOnMemberItem.A1, 1);
        }

        public static void AssertGotSuperPTOnMember(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var superPTOnMember = settingType.SettingItems.ElementAt(0) as SuperPTOnMemberItem;

            Assert.IsNotNull(superPTOnMember);
            Assert.GreaterOrEqual(superPTOnMember.IS1 ? superPTOnMember.US1 : superPTOnMember.S1, 0);
            Assert.GreaterOrEqual(superPTOnMember.M1, 1);
            Assert.GreaterOrEqual(superPTOnMember.A1, 1);
        }

        #endregion Assert Got PT

        #region Modifying PT

        public static ModifySettingTypeInfo ModifySuperPT(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var superPTItem = settingType.SettingItems.ElementAt(0) as SuperPTItem;

            superPTItem.S4 = (byte)(superPTItem.S4 == 90 ? 89 : 90);
            superPTItem.M4 = (byte)(superPTItem.M4 == 88 ? 87 : 88);
            superPTItem.A4 = (byte)(superPTItem.A4 == 73 ? 72 : 73);
            superPTItem.N4 = (byte)(superPTItem.N4 == 3 ? 4 : 3);

            return new ModifySettingTypeInfo
            {
                SettingItem = superPTItem,
                SettingTypeId = SettingTypeInfo.PTSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyMasterPT(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var masterPTItem = settingType.SettingItems.ElementAt(0) as MasterPTItem;

            masterPTItem.IS3 = !masterPTItem.IS3;
            if (masterPTItem.IS3)
            {
                masterPTItem.US3 = (byte)(masterPTItem.US3 == 20 ? 21 : 20);
                masterPTItem.S3 = 0;
            }
            else
            {
                masterPTItem.S3 = (byte)(masterPTItem.S3 == 20 ? 21 : 20);
                masterPTItem.US3 = 0;
            }

            return new ModifySettingTypeInfo
            {
                SettingItem = masterPTItem,
                SettingTypeId = SettingTypeInfo.PTSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyAgentPT(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var agentPT = settingType as PTSettingType;
            var agentPTItem = settingType.SettingItems.ElementAt(0) as AgentPTItem;

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

            return new ModifySettingTypeInfo
            {
                SettingItem = agentPTItem,
                SettingTypeId = SettingTypeInfo.PTSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyAgentPTOnMember(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var agentPTOnMemberItem = settingType.SettingItems.ElementAt(0) as AgentPTOnMemberItem;

            agentPTOnMemberItem.A1 = (byte)(agentPTOnMemberItem.A2 == 20 ? 21 : 20);

            return new ModifySettingTypeInfo
            {
                SettingItem = agentPTOnMemberItem,
                SettingTypeId = SettingTypeInfo.PTSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyMasterPTOnMember(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var masterPTOnMember = settingType as PTSettingType;
            var masterPTOnMemberItem = settingType.SettingItems.ElementAt(0) as MasterPTOnMemberItem;
            if (!masterPTOnMember.IsDisableMasterAutoPT)
            {
                if (masterPTOnMemberItem.IM2)
                {
                    masterPTOnMemberItem.IM1 = true;
                }
                else
                {
                    masterPTOnMemberItem.IM1 = !masterPTOnMemberItem.IM1;
                }
            }

            if (masterPTOnMemberItem.IM1)
            {
                masterPTOnMemberItem.UM1 = (byte)(masterPTOnMemberItem.UM1 == 21 ? 22 : 21);
                masterPTOnMemberItem.M1 = 0;
            }
            else
            {
                masterPTOnMemberItem.M1 = (byte)(masterPTOnMemberItem.M1 == 21 ? 22 : 21);
                masterPTOnMemberItem.UM1 = 0;
            }

            return new ModifySettingTypeInfo
            {
                SettingItem = masterPTOnMemberItem,
                SettingTypeId = SettingTypeInfo.PTSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifySuperPTOnMember(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var superPTOnMemberItem = settingType.SettingItems.ElementAt(0) as SuperPTOnMemberItem;

            if (superPTOnMemberItem.IS3)
            {
                superPTOnMemberItem.IS1 = true;
            }
            else
            {
                superPTOnMemberItem.IS1 = !superPTOnMemberItem.IS1;
            }

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

            return new ModifySettingTypeInfo
            {
                SettingItem = superPTOnMemberItem,
                SettingTypeId = SettingTypeInfo.PTSettingTypeId
            };
        }

        #endregion Modifying PT

        #region Assert Updated PT

        public static void AssertUpdatedSuperPT(ISettingItem superPTItem, ISetting newSuperSetting)
        {
            var settingType = newSuperSetting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var newSuperPTItem = settingType.SettingItems.ElementAt(0) as SuperPTItem;
            var expectedSuperPTItem = superPTItem as SuperPTItem;
            Assert.AreEqual(expectedSuperPTItem.S4, newSuperPTItem.S4);
            Assert.AreEqual(expectedSuperPTItem.M4, newSuperPTItem.M4);
            Assert.AreEqual(expectedSuperPTItem.A4, newSuperPTItem.A4);
            Assert.AreEqual(expectedSuperPTItem.N4, newSuperPTItem.N4);
        }

        public static void AssertUpdatedMasterPT(ISettingItem masterPTItem, ISetting newMasterSetting)
        {
            var settingType = newMasterSetting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var newMasterPTItem = settingType.SettingItems.ElementAt(0) as MasterPTItem;

            var expectedMasterPTItem = masterPTItem as MasterPTItem;
            Assert.AreEqual(expectedMasterPTItem.IS3, newMasterPTItem.IS3);
            Assert.AreEqual(expectedMasterPTItem.US3, newMasterPTItem.US3);
            Assert.AreEqual(expectedMasterPTItem.S3, newMasterPTItem.S3);
            Assert.AreEqual(expectedMasterPTItem.M3, newMasterPTItem.M3);
            Assert.AreEqual(expectedMasterPTItem.N3, newMasterPTItem.N3);
        }

        public static void AssertUpdatedAgentPT(ISettingItem agentPTItem, ISetting newAgentSetting)
        {
            var settingType = newAgentSetting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var newAgentPTItem = settingType.SettingItems.ElementAt(0) as AgentPTItem;

            var expectedAgentPTItem = agentPTItem as AgentPTItem;
            Assert.AreEqual(expectedAgentPTItem.IM2, newAgentPTItem.IM2);
            Assert.AreEqual(expectedAgentPTItem.UM2, newAgentPTItem.UM2);
            Assert.AreEqual(expectedAgentPTItem.M2, newAgentPTItem.M2);
            Assert.AreEqual(expectedAgentPTItem.A2, newAgentPTItem.A2);
            Assert.AreEqual(expectedAgentPTItem.N2, newAgentPTItem.N2);
        }

        public static void AssertUpdatedAgentPTOnMember(ISettingItem agentPTOnMemberItem, ISetting newAgentPTOnMemberSetting)
        {
            var settingType = newAgentPTOnMemberSetting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var newAgentPTOnMemberItem = settingType.SettingItems.ElementAt(0) as AgentPTOnMemberItem;

            var expectedAgentPTOnMemberItem = agentPTOnMemberItem as AgentPTOnMemberItem;

            Assert.AreEqual(expectedAgentPTOnMemberItem.A1, newAgentPTOnMemberItem.A1);
        }

        public static void AssertUpdatedMasterPTOnMember(ISettingItem masterPTOnMemberItem, ISetting newMasterPTOnMemberSetting)
        {
            var settingType = newMasterPTOnMemberSetting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var newMasterPTOnMemberItem = settingType.SettingItems.ElementAt(0) as MasterPTOnMemberItem;

            var expectedMasterPTOnMemberItem = masterPTOnMemberItem as MasterPTOnMemberItem;

            if (expectedMasterPTOnMemberItem.IM1)
            {
                Assert.AreEqual(expectedMasterPTOnMemberItem.UM1, newMasterPTOnMemberItem.UM1);
            }
            else
            {
                Assert.AreEqual(expectedMasterPTOnMemberItem.M1, newMasterPTOnMemberItem.M1);
            }
        }

        public static void AssertUpdatedSuperPTOnMember(ISettingItem superPTOnMemberItem, ISetting newSuperPTOnMemberSetting)
        {
            var settingType = newSuperPTOnMemberSetting.SettingTypes.Where(st => st.Id == SettingTypeInfo.PTSettingTypeId).FirstOrDefault();

            var newSuperPTOnMemberItem = settingType.SettingItems.ElementAt(0) as SuperPTOnMemberItem;

            var expectedSuperPTOnMemberItem = superPTOnMemberItem as SuperPTOnMemberItem;

            if (expectedSuperPTOnMemberItem.IS1)
            {
                Assert.AreEqual(expectedSuperPTOnMemberItem.US1, newSuperPTOnMemberItem.US1);
            }
            else
            {
                Assert.AreEqual(expectedSuperPTOnMemberItem.S1, newSuperPTOnMemberItem.S1);
            }
        }

        #endregion Assert Updated PT
    }
}