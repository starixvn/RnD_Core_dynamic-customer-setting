namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using Fanex.Data;

    public class SettingType1DataAccess : BaseDataAccess, ISettingTypeDataAccess
    {
        private const string SettingKeySuffix = "_1_1";

        public virtual SettingType GetCustomerSettingType(User actor, QuerySettingParam settingQuery)
        {
            var settingType = new SettingType
            {
                Name = Label.PositionTaking
            };

            settingType.SettingItems = ReadSettingItems(settingQuery);
            settingType = ReadIsMasterAutoPTWhenEditAgentPT(settingQuery, settingType);
            return settingType;
        }

        private SettingType ReadIsMasterAutoPTWhenEditAgentPT(QuerySettingParam settingQuery, SettingType settingType)
        {
            if (settingQuery.CustomerLevel == CustomerLevel.Agent)
            {
                bool isAllowMasterPT = ReadIsAllowMasterPT(settingQuery.CustomerId);

                return new MasterPositionTaking
                {
                    Name = settingType.Name,
                    IsDisableMasterAutoPT = !isAllowMasterPT,
                    SettingItems = settingType.SettingItems
                };
            }

            return settingType;
        }

        private bool ReadIsAllowMasterPT(int customerId)
        {
            using (ObjectDb objectDb = new ObjectDb("Acc_CustSetting_AllowMasterPTSelect"))
            {
                return objectDb.QueryEntity<bool>(new { CustID = customerId });
            }
        }

        private IEnumerable<ISettingItem> ReadSettingItems(QuerySettingParam settingQuery)
        {
            var settingKey = GetSettingKeyForGettingSettingType(settingQuery.CustomerLevel, SettingKeySuffix);
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                return QueryPositionTakingItems(objectDb, settingQuery);
            }
        }

        private IEnumerable<BasePositionTakingItem> QueryPositionTakingItems(ObjectDb objectDb, QuerySettingParam settingQuery)
        {
            var storedProcedureParams = GetSPParamsForGettingSettingType(settingQuery, SettingType1Constants.SettingType);

            switch (settingQuery.CustomerLevel)
            {
                case CustomerLevel.Super:
                    return objectDb.Query<SuperPositionTakingItem>(storedProcedureParams);

                case CustomerLevel.Master:
                    return objectDb.Query<MasterPositionTakingItem>(storedProcedureParams);

                case CustomerLevel.Agent:
                    return objectDb.Query<AgentPositionTakingItem>(storedProcedureParams);

                case CustomerLevel.Member:
                    switch (settingQuery.UplineLevel.Value)
                    {
                        case CustomerLevel.Super:
                            return objectDb.Query<SuperOnMemberPositionTakingItem>(storedProcedureParams);

                        case CustomerLevel.Master:
                            return objectDb.Query<MasterOnMemberPositionTakingItem>(storedProcedureParams);

                        case CustomerLevel.Agent:
                            return objectDb.Query<AgentOnMemberPositionTakingItem>(storedProcedureParams);

                        default:
                            throw new InvalidOperationException("This customer level is not supported");
                    }

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }
        }

        public virtual IResult UpdateCustomerSettingType(
            ISettingContext settingContext, 
            Customer customer, 
            SettingType settingType, 
            CustomerLevel? uplineLevel)
        {
            var storedProcedureParams = GetSPParamsForUpdatingPositionTaking(settingContext, customer, settingType);
            var settingKey = GetSettingKeyForUpdatingPositionTaking(customer.Level, uplineLevel);

            return ExecuteNonQuery(settingKey, storedProcedureParams);
        }

        private dynamic GetSPParamsForUpdatingPositionTaking(
            ISettingContext settingContext,
            Customer customer,
            SettingType settingType)
        {
            return new
            {
                UserId = settingContext.Actor.Id,
                SubAccId = settingContext.Actor.SubAccountId,
                CustId = customer.Id,
                Ip = settingContext.IP,
                ErrCode = 0,
                ErrMsg = string.Empty,
                MsgType = 0,
                PT = settingType.ToXml()
            };
        }

        private string GetSettingKeyForUpdatingPositionTaking(CustomerLevel customerLevel, CustomerLevel? uplineLevel)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return "Acc_CustSetting_SuperPTUpd_1_1";

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterPTUpd_1_1";

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentPTUpd_1_1";

                case CustomerLevel.Member:
                    switch (uplineLevel.Value)
                    {
                        case CustomerLevel.Super:
                            return "Acc_CustSetting_SuperPTOnMemberUpd_1_1";

                        case CustomerLevel.Master:
                            return "Acc_CustSetting_MasterPTOnMemberUpd_1_1";

                        case CustomerLevel.Agent:
                            return "Acc_CustSetting_AgentPTOnMemberUpd_1_1";

                        default:
                            throw new InvalidOperationException("This customer level is not supported");
                    }

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }
        }
    }
}