namespace Fanex.CustomerSetting.SettingType10.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Constants;
    using CustomerSetting.DataAccesses.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Data;
    using Dtos;

    internal class DataAccess : BaseDataAccess, IDataAccess
    {
        public IEnumerable<CommissionDto> GetSMAComissions(CustomerSettingQuery query)
        {
            using (var objectDb = new ObjectDb("Acc_CustSetting_SMACommissionGroupSelect"))
            {
                var parameters = GetSPParamsForGettingSettingType(query, Constants.SettingTypeId);

                return objectDb.Query<CommissionDto>(parameters);
            }
        }

        public Tuple<IEnumerable<string>, string, IEnumerable<CommissionDto>> GetMemberComissions(CustomerSettingQuery query)
        {
            IEnumerable<CommissionDto> memberCommissions;
            IEnumerable<string> availableGroups;
            string userGroup = null;

            using (var objectDb = new ObjectDb("Acc_CustSetting_MemberCommissionGroupSelect"))
            {
                var parameters = GetSPParamsForGettingSettingType(query, Constants.SettingTypeId);

                parameters.Add("usergroup", string.Empty);

                using (var multi = objectDb.QueryMultiple(parameters))
                {
                    memberCommissions = multi.Read<CommissionDto>();
                    availableGroups = multi.Read<string>();
                }

                userGroup = parameters["usergroup"].ToString();
            }

            return new Tuple<IEnumerable<string>, string, IEnumerable<CommissionDto>>(
                availableGroups,
                userGroup,
                memberCommissions);
        }

        public IResult UpdateCustomerCommission(ISettingContext settingContext, int customerId, Level customerLevel, Level uplineLevel, string commissionXml)
        {
            var parameters = new Hashtable
            {
                { "userid", settingContext.Actor.Id },
                { "subaccid", settingContext.Actor.SubAccountId },
                { "custid", customerId },
                { "ip", settingContext.IP },
                { "commission", commissionXml },
                { "errcode", 0 },
                { "errmsg", string.Empty },
                { "msgtype", 0 }
            };

            return ExecuteNonQuery("Acc_CustSetting_CommissionGroupUpd", parameters);
        }

        public IEnumerable<CommissionDto> GetMemberComissionGroupConstraints(int memberId, string userGroup)
        {
            using (var objectDb = new ObjectDb("Acc_CustSetting_MemberCommissionGroupConstraintsSelect"))
            {
                var agentCommissionGroupValues = objectDb.Query<CommissionDto>(new
                {
                    CustId = memberId,
                    UserGroup = userGroup
                });

                return agentCommissionGroupValues;
            }
        }
    }
}