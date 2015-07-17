namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos;
    using Fanex.Data;

    public class MemberGroupDataAccess : BaseDataAccess, IMemberGroupDataAccess
    {
        private static readonly Dictionary<byte, string> UpdateMultipleSPKeys = new Dictionary<byte, string>
        {
            { UserLevel.Super.Value, "CustSetting_SuperPTOnMemberMultiUpd" },
            { UserLevel.Master.Value, "CustSetting_MasterPTOnMemberMultiUpd" },
            { UserLevel.Agent.Value, "CustSetting_AgentPTOnMemberMultiUpd" }
        };

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

        public IResult UpdateGroupedMembersSetting(UpdateMultipleMemberPTParam updateParam)
        {
            if (updateParam == null)
            {
                throw new ArgumentNullException("updateParam");
            }

            var settingKey = UpdateMultipleSPKeys[updateParam.UplineLevel.Value];
            var storedProcedureParams = GetSPParamsOfUpdateGroupedMembersSetting(updateParam);

            return ExecuteNonQuery(settingKey, storedProcedureParams);
        }

        private Hashtable GetSPParamsOfUpdateGroupedMembersSetting(UpdateMultipleMemberPTParam updateParam)
        {
            Hashtable param = new Hashtable();
            param.Add("userid", updateParam.UserId);
            param.Add("subaccid", updateParam.SubAccId);
            param.Add("custidlist", updateParam.CustIds);
            param.Add("pt", updateParam.PTXml);
            param.Add("ip", updateParam.IP);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return param;
        }
    }
}