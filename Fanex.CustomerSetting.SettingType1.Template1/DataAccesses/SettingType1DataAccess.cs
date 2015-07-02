namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using Fanex.Data;

    public class SettingType1DataAccess : BaseDataAccess, ISettingTypeDataAccess
    {
        public virtual SettingType GetCustomerSettingType(User actor, QuerySettingParam queryParam)
        {
            var settingType = new SettingType
            {
                Name = Label.PositionTaking
            };

            settingType.SettingItems = ReadSettingItems(queryParam);
            settingType = ReadIsMasterAutoPTWhenEditAgentPT(queryParam, settingType);
            return settingType;
        }

        private SettingType ReadIsMasterAutoPTWhenEditAgentPT(QuerySettingParam queryParam, SettingType settingType)
        {
            if (queryParam.CustomerLevel == CustomerLevel.Agent)
            {
                bool isAllowMasterPT = ReadIsAllowMasterPT(queryParam.CustomerId);

                return new MasterPT
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
            using (ObjectDb objectDb = new ObjectDb("CustSetting_AllowMasterPTSelect"))
            {
                return objectDb.QueryEntity<bool>(new { CustID = customerId });
            }
        }

        private IEnumerable<ISettingItem> ReadSettingItems(QuerySettingParam queryParam)
        {
            var settingKey = GetSettingKeyForGettingSettingType(queryParam, PTConstants.SettingTypeId, PTConstants.SettingTempateId);
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                return QueryPTItems(objectDb, queryParam);
            }
        }

        private IEnumerable<BasePTItem> QueryPTItems(ObjectDb objectDb, QuerySettingParam queryParam)
        {
            var storedProcedureParams = GetSPParamsForGettingSettingType(queryParam, PTConstants.SettingTypeId);

            switch (queryParam.CustomerLevel)
            {
                case CustomerLevel.Super:
                    return objectDb.Query<SuperPTItem>(storedProcedureParams);

                case CustomerLevel.Master:
                    return objectDb.Query<MasterPTItem>(storedProcedureParams);

                case CustomerLevel.Agent:
                    return objectDb.Query<AgentPTItem>(storedProcedureParams);

                case CustomerLevel.Member:
                    switch (queryParam.UplineLevel.Value)
                    {
                        case CustomerLevel.Super:
                            return objectDb.Query<SuperPTOnMemberItem>(storedProcedureParams);

                        case CustomerLevel.Master:
                            return objectDb.Query<MasterPTOnMemberItem>(storedProcedureParams);

                        case CustomerLevel.Agent:
                            return objectDb.Query<AgentPTOnMemberItem>(storedProcedureParams);

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
            byte productId,
            CustomerLevel? uplineLevel)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            var storedProcedureParams = GetSPParamsForUpdatingPT(settingContext, customer, settingType);
            var settingKey = GetSettingKeyForUpdatingPT(customer.Level, uplineLevel, settingType.Id, settingType.SettingTemplate, productId);

            return ExecuteNonQuery(settingKey, storedProcedureParams);
        }

        private Hashtable GetSPParamsForUpdatingPT(
            ISettingContext settingContext,
            Customer customer,
            SettingType settingType)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customer.Id);
            param.Add("ip", settingContext.IP);
            param.Add("pt", settingType.ToXml());
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return param;
        }

        private string GetSettingKeyForUpdatingPT(
            CustomerLevel customerLevel,
            CustomerLevel? uplineLevel,
            short settingTypeId,
            byte settingTemplateId,
            byte productId)
        {
            var updateSettingKey = string.Empty;
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    updateSettingKey = "CustSetting_SuperPTUpd_1_1";
                    break;

                case CustomerLevel.Master:
                    updateSettingKey = "CustSetting_MasterPTUpd_1_1";
                    break;

                case CustomerLevel.Agent:
                    updateSettingKey = "CustSetting_AgentPTUpd_1_1";
                    break;

                case CustomerLevel.Member:
                    switch (uplineLevel.Value)
                    {
                        case CustomerLevel.Super:
                            updateSettingKey = "CustSetting_SuperPTOnMemberUpd_1_1";
                            break;

                        case CustomerLevel.Master:
                            updateSettingKey = "CustSetting_MasterPTOnMemberUpd_1_1";
                            break;

                        case CustomerLevel.Agent:
                            updateSettingKey = "CustSetting_AgentPTOnMemberUpd_1_1";
                            break;

                        default:
                            throw new InvalidOperationException("This customer level is not supported");
                    }

                    break;

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }

            if (CustomerSettingSection.Settings.IsProductNotUseDefaultStoredProcedures(settingTypeId, settingTemplateId, productId))
            {
                updateSettingKey += string.Format("_{0}", productId);
            }

            return updateSettingKey;
        }
    }
}