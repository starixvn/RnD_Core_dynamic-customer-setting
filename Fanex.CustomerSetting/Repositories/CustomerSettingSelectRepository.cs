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

        /// <summary>
        /// Super select master setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer Setting Entity.</returns>
        public SuperSelectMasterCustomerSettingEntity SuperSelectMaster(SuperSelectMasterCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<SuperSelectMasterPositionTakingEntity>();
                    var betLimits = multi.Read<SuperSelectMasterBetLimitEntity>();
                    var discounts = multi.Read<SuperSelectMasterDiscountEntity>();
                    var statuses = multi.Read<SuperSelectMasterProductStatusEntity>();
                    var setting = multi.Read<ProductSetting>().First();
                    return new SuperSelectMasterCustomerSettingEntity
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

        /// <summary>
        /// Master select agent setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer Setting Entity.</returns>
        public MasterSelectAgentCustomerSettingEntity MasterSelectAgent(MasterSelectAgentCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<MasterSelectAgentPositionTakingEntity>();
                    var betLimits = multi.Read<MasterSelectAgentBetLimitEntity>();
                    var discounts = multi.Read<MasterSelectAgentDiscountEntity>();
                    var statuses = multi.Read<MasterSelectAgentProductStatusEntity>();
                    var setting = multi.Read<ProductSetting>().First();
                    return new MasterSelectAgentCustomerSettingEntity
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

        /// <summary>
        /// Super select member setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer Setting Entity.</returns>
        public SuperSelectMemberCustomerSettingEntity SuperSelectMember(SuperSelectMemberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<SuperSelectMemberPositionTakingEntity>();
                    var betLimits = multi.Read<SuperSelectMemberBetLimitEntity>();
                    var discounts = multi.Read<SuperSelectMemberDiscountEntity>();
                    var statuses = multi.Read<SuperSelectMemberProductStatusEntity>();
                    var setting = multi.Read<ProductSetting>().First();
                    return new SuperSelectMemberCustomerSettingEntity
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

        /// <summary>
        /// Master select member setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer Setting Entity.</returns>
        public MasterSelectMemberCustomerSettingEntity MasterSelectMember(SuperSelectMemberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<MasterSelectMemberPositionTakingEntity>();
                    var betLimits = multi.Read<MasterSelectMemberBetLimitEntity>();
                    var discounts = multi.Read<MasterSelectMemberDiscountEntity>();
                    var statuses = multi.Read<MasterSelectMemberProductStatusEntity>();
                    var setting = multi.Read<ProductSetting>().First();
                    return new MasterSelectMemberCustomerSettingEntity
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

        /// <summary>
        /// Agent select member setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer Setting Entity.</returns>
        public AgentSelectMemberCustomerSettingEntity AgentSelectMember(SuperSelectMemberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<AgentSelectMemberPositionTakingEntity>();
                    var betLimits = multi.Read<AgentSelectMemberBetLimitEntity>();
                    var discounts = multi.Read<AgentSelectMemberDiscountEntity>();
                    var statuses = multi.Read<AgentSelectMemberProductStatusEntity>();
                    var setting = multi.Read<ProductSetting>().First();
                    return new AgentSelectMemberCustomerSettingEntity
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
    }
}