namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;

    /// <summary>
    /// ValidationService class.
    /// </summary>
    public class ValidationService : IValidationService
    {
        #region Variables

        private readonly IProductProfileService _productProfileService;
        private readonly IValidationRepository _validationRepository;

        #endregion Variables

        #region Constructors

        public ValidationService()
            : this(null, null)
        {
        }

        public ValidationService(IProductProfileService productProfileService, IValidationRepository validationRepository)
        {
            _productProfileService = productProfileService ?? new ProductProfileService();
            _validationRepository = validationRepository ?? new ValidationRepository();
        }

        #endregion Constructors

        /// <summary>
        /// Validate select model.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Returns model validation.</returns>
        public Result Validate(QueryInfo model)
        {
            var result = ValidateRole(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidatePermission(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            return Result.Success;
        }

        /// <summary>
        /// Validate update model.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns model validation.</returns>
        public Result Validate(CommandInfo model)
        {
            var result = ValidateForm(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidateRole(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidatePermission(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidateProductProfile(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            return Result.Success;
        }

        private Result ValidateForm(CommandInfo model)
        {
            return (model.FormType == FormType.SuperEditMember || model.FormType == FormType.MasterEditMember)
                && model.Discounts != null
                && model.Discounts.Count() > 0
                ? Result.AccessDenied
                : Result.Success;
        }

        /// <summary>
        /// Validates the user role.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Returns model validation.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Switch has multiple cases.")]
        private Result ValidateRole(QueryInfo model)
        {
            switch (model.FormType)
            {
                case FormType.CompanyEditSuper:
                    if (model.RoleId != RoleType.Company || model.TargetRoleId != RoleType.Super)
                    {
                        return Result.AccessDenied;
                    }

                    break;

                case FormType.SuperEditMaster:
                    if ((model.RoleId != RoleType.Company && model.RoleId != RoleType.Super) || model.TargetRoleId != RoleType.Master)
                    {
                        return Result.AccessDenied;
                    }

                    break;

                case FormType.MasterEditAgent:
                    if ((model.RoleId != RoleType.Company && model.RoleId != RoleType.Master) || model.TargetRoleId != RoleType.Agent)
                    {
                        return Result.AccessDenied;
                    }

                    break;

                case FormType.SuperEditMember:
                    if ((model.RoleId != RoleType.Company && model.RoleId != RoleType.Super) || model.TargetRoleId != RoleType.Member)
                    {
                        return Result.AccessDenied;
                    }

                    break;

                case FormType.MasterEditMember:
                    if ((model.RoleId != RoleType.Company && model.RoleId != RoleType.Master) || model.TargetRoleId != RoleType.Member)
                    {
                        return Result.AccessDenied;
                    }

                    break;

                case FormType.AgentEditMember:
                    if ((model.RoleId != RoleType.Company && model.RoleId != RoleType.Agent) || model.TargetRoleId != RoleType.Member)
                    {
                        return Result.AccessDenied;
                    }

                    break;
            }

            return Result.Success;
        }

        /// <summary>
        /// Validate status of customer and target customer to be open.
        /// Validate target customer to have the exacted target customer role type.
        /// Validate target customer to be down-line of customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns validation result.</returns>
        private Result ValidatePermission(QueryInfo model)
        {
            var criteria = new PermissionValidateCriteria
            {
                CustId = model.CustId,
                TargetCustId = model.TargetCustId,
                TargetRoleId = (int)model.TargetRoleId
            };

            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _validationRepository.ValidatePermission(criteria);
            return new Result(criteria.ErrCode, criteria.ErrMsg);
        }

        /// <summary>
        /// Validate the product profile.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns product profile validation.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Switch has multiple cases.")]
        private Result ValidateProductProfile(CommandInfo model)
        {
            var productProfile = _productProfileService.GetProductProfile(model.ProductId);
            switch (model.SettingType)
            {
                case SettingType.All:
                    IEnumerable<BaseEntity>[] dataList =
                    {
                        model.PositionTakings,
                        model.BetLimits,
                        model.Discounts,
                        model.Statuses
                    };

                    var hasData = false;
                    foreach (var data in dataList)
                    {
                        if (data != null && data.Count() > 0)
                        {
                            hasData = true;
                            break;
                        }
                    }

                    if (!hasData)
                    {
                        return Result.NoChange;
                    }

                    foreach (var data in dataList)
                    {
                        var result = ValidateProductProfile(data, productProfile);
                        if (!result.IsSuccess)
                        {
                            return result;
                        }
                    }

                    return Result.Success;

                case SettingType.PositionTaking:
                    if (model.PositionTakings == null || model.PositionTakings.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(model.PositionTakings, productProfile);

                case SettingType.BetLimit:
                    if (model.BetLimits == null || model.BetLimits.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(model.BetLimits, productProfile);

                case SettingType.Discount:
                    if (model.Discounts == null || model.Discounts.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(model.Discounts, productProfile);

                case SettingType.Status:
                    if (model.Statuses == null || model.Statuses.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    return ValidateProductProfile(model.Statuses, productProfile);
            }

            return Result.Success;
        }

        /// <summary>
        /// Validate the product profile.
        /// </summary>
        /// <param name="entities">The validated data.</param>
        /// <param name="productProfile">The product profile.</param>
        /// <returns>Returns product profile validation.</returns>
        private Result ValidateProductProfile(IEnumerable<BaseEntity> entities, ProductProfileEntity productProfile)
        {
            if (productProfile == null || productProfile.Items.Count() == 0)
            {
                return Result.Error;
            }

            if (entities == null)
            {
                return Result.Success;
            }

            foreach (var entity in entities)
            {
                if (!entity.IsValid() || !productProfile.Items.Where(profileItem => profileItem.SportType == entity.SportType && profileItem.BetType == entity.BetType).Any())
                {
                    return Result.AccessDenied;
                }
            }

            return Result.Success;
        }
    }
}