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

            var settingKey = GetSettingKeyForQuerying(
                queryParam.CustomerLevel,
                PTConstants.SettingTypeId,
                PTConstants.SettingTempateId,
                queryParam.ProductId);

            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                return QueryCustomerPT(objectDb, queryParam, settingType);
            }
        }

        private string GetSettingKeyForQuerying(
            CustomerLevel customerLevel, 
            byte settingTypeId, 
            byte settingTemplateId, 
            byte productId)
        {
            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(settingTypeId, settingTemplateId, productId);

            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return "CustSetting_SuperPTSelect" + keySuffix;

                case CustomerLevel.Master:
                    return "CustSetting_MasterPTSelect" + keySuffix;

                case CustomerLevel.Agent:
                    return "CustSetting_AgentPTSelect" + keySuffix;

                case CustomerLevel.Member:
                    return "CustSetting_MemberPTSelect" + keySuffix;

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }
        }

        private SettingType QueryCustomerPT(ObjectDb objectDb, QuerySettingParam queryParam, SettingType settingType)
        {
            var parameters = GetSPParamsForGettingSettingType(queryParam, PTConstants.SettingTypeId);

            switch (queryParam.CustomerLevel)
            {
                case CustomerLevel.Super:
                    settingType.SettingItems = objectDb.Query<SuperPTItem>(parameters);
                    return settingType;

                case CustomerLevel.Master:
                    settingType.SettingItems = objectDb.Query<MasterPTItem>(parameters);
                    return settingType;

                case CustomerLevel.Agent:
                    return QueryAgentPT(objectDb, settingType, parameters);

                case CustomerLevel.Member:
                    switch (queryParam.UplineLevel.Value)
                    {
                        case CustomerLevel.Super:
                            settingType.SettingItems = objectDb.Query<SuperPTOnMemberItem>(parameters);
                            return settingType;

                        case CustomerLevel.Master:
                            return QueryMasterPTOnMember(objectDb, settingType, parameters);

                        case CustomerLevel.Agent:
                            settingType.SettingItems = objectDb.Query<AgentPTOnMemberItem>(parameters);
                            return settingType;

                        default:
                            throw new InvalidOperationException("This customer level is not supported");
                    }

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }
        }

        private SettingType QueryAgentPT(ObjectDb objectDb, SettingType settingType, Hashtable parameters)
        {
            parameters.Add("disableautompt", null);
            settingType.SettingItems = objectDb.Query<AgentPTItem>(parameters);
            AgentPT agentPT = new AgentPT
            {
                Id = settingType.Id,
                Name = settingType.Name,
                SettingLevel = settingType.SettingLevel,
                SettingTemplate = settingType.SettingTemplate,
                IsDisableMasterAutoPT = Convert.ToBoolean(parameters["disableautompt"]),
                SettingItems = settingType.SettingItems
            };

            return agentPT;
        }

        private MasterPTOnMember QueryMasterPTOnMember(ObjectDb objectDb, SettingType settingType, Hashtable parameters)
        {
            parameters.Add("disableautompt", null);
            settingType.SettingItems = objectDb.Query<MasterPTOnMemberItem>(parameters);
            MasterPTOnMember masterPTOnMember = new MasterPTOnMember
            {
                Id = settingType.Id,
                Name = settingType.Name,
                SettingLevel = settingType.SettingLevel,
                SettingTemplate = settingType.SettingTemplate,
                IsDisableMasterAutoPT = Convert.ToBoolean(parameters["disableautompt"]),
                SettingItems = settingType.SettingItems
            };

            return masterPTOnMember;
        }

        public virtual IResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            CustomerLevel customerLevel,
            SettingType settingType,
            byte productId,
            CustomerLevel? uplineLevel)
        {
            var parameters = GetSPParamsForUpdatingPT(settingContext, customerId, settingType);
            var settingKey = GetSettingKeyForUpdating(customerLevel, uplineLevel, productId);

            return ExecuteNonQuery(settingKey, parameters);
        }

        private Hashtable GetSPParamsForUpdatingPT(
            ISettingContext settingContext,
            int customerId,
            SettingType settingType)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customerId);
            param.Add("ip", settingContext.IP);
            param.Add("pt", settingType.ToXml());
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return param;
        }

        private string GetSettingKeyForUpdating(
            CustomerLevel customerLevel,
            CustomerLevel? uplineLevel,
            byte productId)
        {
            var updateSettingKey = string.Empty;
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    updateSettingKey = "CustSetting_SuperPTUpd";
                    break;

                case CustomerLevel.Master:
                    updateSettingKey = "CustSetting_MasterPTUpd";
                    break;

                case CustomerLevel.Agent:
                    updateSettingKey = "CustSetting_AgentPTUpd";
                    break;

                case CustomerLevel.Member:
                    switch (uplineLevel.Value)
                    {
                        case CustomerLevel.Super:
                            updateSettingKey = "CustSetting_SuperPTOnMemberUpd";
                            break;

                        case CustomerLevel.Master:
                            updateSettingKey = "CustSetting_MasterPTOnMemberUpd";
                            break;

                        case CustomerLevel.Agent:
                            updateSettingKey = "CustSetting_AgentPTOnMemberUpd";
                            break;

                        default:
                            throw new InvalidOperationException("This customer level is not supported");
                    }

                    break;

                default:
                    throw new InvalidOperationException("This customer level is not supported");
            }

            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(
                PTConstants.SettingTypeId,
                PTConstants.SettingTempateId,
                productId);

            return updateSettingKey + keySuffix;
        }
    }
}