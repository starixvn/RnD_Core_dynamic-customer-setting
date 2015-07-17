namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
        private static readonly Dictionary<byte, string> UpdateSmaPTSpKeys = new Dictionary<byte, string> 
        {
            { UserLevel.Super.Value, "CustSetting_SuperPTUpd" },
            { UserLevel.Master.Value, "CustSetting_MasterPTUpd" },
            { UserLevel.Agent.Value, "CustSetting_AgentPTUpd" }
        };

        private static readonly Dictionary<byte, string> UpdateMemberPTSpKeys = new Dictionary<byte, string> 
        {
            { UserLevel.Super.Value, "CustSetting_SuperPTOnMemberUpd" },
            { UserLevel.Master.Value, "CustSetting_MasterPTOnMemberUpd" },
            { UserLevel.Agent.Value, "CustSetting_AgentPTOnMemberUpd" }
        };

        private static readonly Dictionary<byte, string> QueryPTSpKeys = new Dictionary<byte, string> 
        {
            { UserLevel.Super.Value, "CustSetting_SuperPTSelect" },
            { UserLevel.Master.Value, "CustSetting_MasterPTSelect" },
            { UserLevel.Agent.Value, "CustSetting_AgentPTSelect" },
            { UserLevel.Member.Value, "CustSetting_MemberPTSelect" }
        };

        private static readonly Dictionary<byte, Func<ObjectDb, Hashtable, IEnumerable<BasePTItem>>>
            SmaQueryPTSettingItemFuncs = new Dictionary<byte, Func<ObjectDb, Hashtable, IEnumerable<BasePTItem>>>
            {
                { UserLevel.Super.Value, GetPTSettingItem<SuperPTItem> },
                { UserLevel.Master.Value,  GetPTSettingItem<MasterPTItem> },
                { UserLevel.Agent.Value,  GetPTSettingItem<AgentPTItem> }
            };

        private static readonly Dictionary<byte, Func<ObjectDb, Hashtable, IEnumerable<BasePTItem>>>
           MemberQueryPTSettingItemFuncs = new Dictionary<byte, Func<ObjectDb, Hashtable, IEnumerable<BasePTItem>>>
            {
                { UserLevel.Super.Value, GetPTSettingItem<SuperPTOnMemberItem> },
                { UserLevel.Master.Value,  GetPTSettingItem<MasterPTOnMemberItem> },
                { UserLevel.Agent.Value,  GetPTSettingItem<AgentPTOnMemberItem> }
            };

        private static IEnumerable<T> GetPTSettingItem<T>(ObjectDb objectDb, Hashtable parameters)
        {
            return objectDb.Query<T>(parameters);
        }

        public virtual SettingType GetCustomerSettingType(User actor, QuerySettingParam querySettingParam)
        {
            var settingType = new PTSettingType
            {
                Name = Label.PositionTaking
            };

            var settingKey = CreateQueryPTSPSettingKey(
                querySettingParam.CustomerLevel, 
                PTConstants.SettingTypeId,
                PTConstants.SettingTempateId, 
                querySettingParam.ProductId);

            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameters = CreateQueryPTSettingSpParam(querySettingParam);

                var queryFunction = (querySettingParam.CustomerLevel == UserLevel.Member)
                   ? MemberQueryPTSettingItemFuncs[querySettingParam.UplineLevel.Value]
                   : SmaQueryPTSettingItemFuncs[querySettingParam.CustomerLevel.Value];
                
                settingType.SettingItems = queryFunction(objectDb, parameters);

                if (parameters.ContainsKey("disableautompt"))
                {
                    settingType.IsDisableMasterAutoPT = Convert.ToBoolean(parameters["disableautompt"]);
                }
            }

            return settingType;
        }

        private string CreateQueryPTSPSettingKey(UserLevel customerLevel, byte settingTypeId, byte settingTemplateId, byte productId)
        {
            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(settingTypeId, settingTemplateId, productId);

            return QueryPTSpKeys[customerLevel.Value] + keySuffix;
        }

        private Hashtable CreateQueryPTSettingSpParam(QuerySettingParam querySettingParam)
        {
            var parameters = this.GetSPParamsForGettingSettingType(querySettingParam, PTConstants.SettingTypeId);

            if (querySettingParam.CustomerLevel == UserLevel.Agent
                || querySettingParam.UplineLevel == UserLevel.Master)
            {
                parameters.Add("disableautompt", null);
            }

            return parameters;
        }

        public virtual IResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            UserLevel customerLevel,
            SettingType settingType,
            byte productId,
            UserLevel uplineLevel)
        {
            var parameters = GetSPParamsForUpdatingPT(settingContext, customerId, settingType);
            var settingKey = GetSettingKeyForUpdating(customerLevel, uplineLevel, productId);

            return ExecuteNonQuery(settingKey, parameters);
        }

        private Hashtable GetSPParamsForUpdatingPT(ISettingContext settingContext, int customerId, SettingType settingType)
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

        private string GetSettingKeyForUpdating(UserLevel customerLevel, UserLevel uplineLevel, byte productId)
        {
            var updateSettingKey = (customerLevel == UserLevel.Member)
                ? UpdateMemberPTSpKeys[uplineLevel.Value]
                : UpdateSmaPTSpKeys[customerLevel.Value];

            var keySuffix = GetKeySuffixOfProductDoesNotUserDefaultSettingTypeSps(
                PTConstants.SettingTypeId,
                PTConstants.SettingTempateId,
                productId);

            return updateSettingKey + keySuffix;
        }
    }
}