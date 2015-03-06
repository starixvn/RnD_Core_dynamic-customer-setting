namespace Fanex.CustomerSetting.Repositories
{
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.Data;

    /// <summary>
    /// CustomerSettingRepository class.
    /// </summary>
    public class CustomerSettingSelectRepository : ICustomerSettingSelectRepository
    {
        /// <summary>
        /// Company select super setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer Setting Entity.</returns>
        public CompanySelectSuperCustomerSettingEntity CompanySelectSuper(CompanySelectSuperCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<CompanySelectSuperPositionTakingEntity>();
                    var betLimits = multi.Read<CompanySelectSuperBetLimitEntity>();
                    var discounts = multi.Read<CompanySelectSuperDiscountEntity>();
                    var statuses = multi.Read<CompanySelectSuperProductStatusEntity>();
                    var setting = multi.Read<ProductSetting>().First();
                    return new CompanySelectSuperCustomerSettingEntity
                    {
                        Information = customerInformation,
                        PositionTakings = positionTakings,
                        BetLimits = betLimits,
                        Discounts = discounts,
                        Statuses = statuses,
                        Setting = setting
                    };
                }
            }
        }

        public SuperSelectMasterCustomerSettingEntity SuperSelectMaster(SuperSelectMasterCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                return null;
            }
        }

        public MasterSelectAgentCustomerSettingEntity MasterSelectAgent(MasterSelectAgentCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                return null;
            }
        }

        public SuperSelectMemberCustomerSettingEntity SuperSelectMember(SuperSelectMemberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                return null;
            }
        }

        public MasterSelectMemberCustomerSettingEntity MasterSelectMember(SuperSelectMemberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                return null;
            }
        }

        public AgentSelectMemberCustomerSettingEntity AgentSelectMember(SuperSelectMemberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                return null;
            }
        }
    }
}