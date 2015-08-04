namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using Fanex.Data;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        private static readonly Dictionary<byte, string> UpdateSmaPTSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperPTUpd" },
            { Level.Master.Value, "CustSetting_MasterPTUpd" },
            { Level.Agent.Value, "CustSetting_AgentPTUpd" }
        };

        private static readonly Dictionary<byte, string> UpdateMemberPTSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperPTOnMemberUpd" },
            { Level.Master.Value, "CustSetting_MasterPTOnMemberUpd" },
            { Level.Agent.Value, "CustSetting_AgentPTOnMemberUpd" }
        };

        private static readonly Dictionary<byte, string> QueryPTSpKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperPTSelect" },
            { Level.Master.Value, "CustSetting_MasterPTSelect" },
            { Level.Agent.Value, "CustSetting_AgentPTSelect" },
            { Level.Member.Value, "CustSetting_MemberPTSelect" }
        };

        private static readonly Dictionary<byte, Func<ObjectDb, Hashtable, IEnumerable<PTItem>>>
            SmaQueryPTSettingItemFuncs = new Dictionary<byte, Func<ObjectDb, Hashtable, IEnumerable<PTItem>>>
            {
                { Level.Super.Value, GetPTSettingItem<SuperPTItem> },
                { Level.Master.Value,  GetPTSettingItem<MasterPTItem> },
                { Level.Agent.Value,  GetPTSettingItem<AgentPTItem> }
            };

        private static readonly Dictionary<byte, Func<ObjectDb, Hashtable, IEnumerable<PTItem>>>
           MemberQueryPTSettingItemFuncs = new Dictionary<byte, Func<ObjectDb, Hashtable, IEnumerable<PTItem>>>
            {
                { Level.Super.Value, GetPTSettingItem<SuperPTOnMemberItem> },
                { Level.Master.Value,  GetPTSettingItem<MasterPTOnMemberItem> },
                { Level.Agent.Value,  GetPTSettingItem<AgentPTOnMemberItem> }
            };

        private static readonly Dictionary<byte, string> UpdateMultiplePTOnMembersSPKeys = new Dictionary<byte, string>
        {
            { Level.Super.Value, "CustSetting_SuperPTOnMemberMultiUpd" },
            { Level.Master.Value, "CustSetting_MasterPTOnMemberMultiUpd" },
            { Level.Agent.Value, "CustSetting_AgentPTOnMemberMultiUpd" }
        };

        private static IEnumerable<T> GetPTSettingItem<T>(ObjectDb objectDb, Hashtable parameters)
        {
            return objectDb.Query<T>(parameters);
        }

        public SettingType GetCustomerPT(CustomerSettingQuery query)
        {
            var settingType = new PTSettingType
            {
                Name = Label.PositionTaking
            };

            var settingKey = QueryPTSpKeys[query.CustomerLevel.Value];

            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                var parameters = CreateQueryPTSettingSpParam(query);

                var queryFunction = (query.CustomerLevel == Level.Member)
                   ? MemberQueryPTSettingItemFuncs[query.UplineLevel.Value]
                   : SmaQueryPTSettingItemFuncs[query.CustomerLevel.Value];

                settingType.SettingItems = queryFunction(objectDb, parameters);

                if (parameters.ContainsKey("disableautompt"))
                {
                    settingType.IsDisableMasterAutoPT = Convert.ToBoolean(parameters["disableautompt"]);
                }
            }

            return settingType;
        }

        private Hashtable CreateQueryPTSettingSpParam(CustomerSettingQuery query)
        {
            var parameters = this.GetSPParamsForGettingSettingType(query, Constants.SettingTypeId);

            if (query.CustomerLevel == Level.Agent
                || query.UplineLevel == Level.Master)
            {
                parameters.Add("disableautompt", null);
            }

            return parameters;
        }

        public IResult UpdateCustomerPT(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string positionTakingXml)
        {
            var parameters = GetSPParamsForUpdatingPT(settingContext, customerId, positionTakingXml);
            var settingKey = (customerLevel == Level.Member)
                ? UpdateMemberPTSpKeys[uplineLevel.Value]
                : UpdateSmaPTSpKeys[customerLevel.Value];

            return ExecuteNonQuery(settingKey, parameters);
        }

        private Hashtable GetSPParamsForUpdatingPT(ISettingContext settingContext, int customerId, string positionTakingXml)
        {
            var param = new Hashtable();
            param.Add("userid", settingContext.Actor.Id);
            param.Add("subaccid", settingContext.Actor.SubAccountId);
            param.Add("custid", customerId);
            param.Add("ip", settingContext.IP);
            param.Add("pt", positionTakingXml);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return param;
        }

        public IEnumerable<int> GetGroupedMemberIds(string memberIds)
        {
            using (ObjectDb objectDb = new ObjectDb("CustSetting_GroupedPTMemberLstSelect"))
            {
                return objectDb.Query<int>(new
                {
                    CustIDList = memberIds
                });
            }
        }

        public IResult UpdateGroupedMembersSetting(
            int actorId,
            int subAccountId,
            string groupedMemberIds,
            Level uplineLevel,
            string positionTakingXml,
            string ip)
        {
            var settingKey = UpdateMultiplePTOnMembersSPKeys[uplineLevel.Value];

            var param = new Hashtable();
            param.Add("userid", actorId);
            param.Add("subaccid", subAccountId);
            param.Add("custidlist", groupedMemberIds);
            param.Add("pt", positionTakingXml);
            param.Add("ip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return ExecuteNonQuery(settingKey, param);
        }
    }
}