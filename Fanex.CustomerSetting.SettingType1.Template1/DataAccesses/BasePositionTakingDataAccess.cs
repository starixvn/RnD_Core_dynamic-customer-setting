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
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using Fanex.Data;

    public class BasePositionTakingDataAccess : BaseDataAccess, ISettingDataAccess
    {
        public virtual SettingType GetCustomerSettingType(User actor, SettingQuery settingQuery)
        {
            var settingKey = GetSettingKeyForGettingPositionTaking(settingQuery.CustomerLevel);
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                return new SettingType
                {
                    SettingItems = QueryPositionTakingItems(objectDb, settingQuery)
                };
            }
        }

        private string GetSettingKeyForGettingPositionTaking(CustomerLevel customerLevel)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return "Acc_CustSetting_SuperSettingSelect";

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterSettingSelect";

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentSettingSelect";

                case CustomerLevel.Member:
                    return "Acc_CustSetting_MemberSettingSelect";

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }

        private IEnumerable<BasePositionTakingItem> QueryPositionTakingItems(ObjectDb objectDb, SettingQuery settingQuery)
        {
            var storedProcedureParams = GetSPParamsForGettingPositionTaking(settingQuery);

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
                            throw new ArgumentOutOfRangeException("This customer level is not supported");
                    }

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }

        private dynamic GetSPParamsForGettingPositionTaking(SettingQuery settingQuery)
        {
            switch (settingQuery.CustomerLevel)
            {
                case CustomerLevel.Super:
                case CustomerLevel.Master:
                case CustomerLevel.Agent:
                    return new
                    {
                        CustId = settingQuery.CustomerId,
                        ProductId = settingQuery.ProductId,
                        SettingType = SettingType1Constants.SettingType
                    };

                case CustomerLevel.Member:
                    return new
                    {
                        CustId = settingQuery.CustomerId,
                        ProductId = settingQuery.ProductId,
                        RoleId = settingQuery.UplineLevel.Value,
                        SettingType = SettingType1Constants.SettingType
                    };

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }

        public virtual IResult UpdateCustomerSetting(
            ISettingContext settingContext, 
            Customer customer, 
            SettingType settingType, 
            CustomerLevel? uplineLevel)
        {
            var storedProcedureParams = GetSPParamsForUpdatingPositionTaking(settingContext, customer, settingType, uplineLevel);
            var settingKey = GetSettingKeyForUpdatingPositionTaking(customer.Level, uplineLevel);

            return ExecuteNonQuery(settingKey, storedProcedureParams);
        }

        private dynamic GetSPParamsForUpdatingPositionTaking(
            ISettingContext settingContext,
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel)
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
                    return "Acc_CustSetting_SuperPTUpd";

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterPTUpd";

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentPTUpd";

                case CustomerLevel.Member:
                    switch (uplineLevel.Value)
                    {
                        case CustomerLevel.Super:
                            return "Acc_CustSetting_SuperPTOnMemberUpd";

                        case CustomerLevel.Master:
                            return "Acc_CustSetting_MasterPTOnMemberUpd";

                        case CustomerLevel.Agent:
                            return "Acc_CustSetting_AgentPTOnMemberUpd";

                        default:
                            throw new ArgumentOutOfRangeException("This customer level is not supported");
                    }

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }
    }
}