namespace Fanex.CustomerSetting.Services
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    /// <summary>
    /// ValidationService class.
    /// </summary>
    public class ValidationService : IValidationService
    {
        private IProductProfileService _productProfileService;

        public ValidationService()
            : this(null)
        {
        }

        public ValidationService(IProductProfileService productProfileService)
        {
            _productProfileService = productProfileService ?? new ProductProfileService();
        }

        /// <summary>
        /// Validate select model.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Returns model validation.</returns>
        public Result Validate(SelectModel model)
        {
            var result = ValidateRole(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidateDownline(model);
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
        public Result Validate(UpdateModel model)
        {
            var result = ValidateRole(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            result = ValidateDownline(model);
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

        /// <summary>
        /// Validates the user role.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Returns model validation.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Switch has multiple cases.")]
        private Result ValidateRole(BaseModel model)
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
                    if (model.TargetRoleId != RoleType.Master)
                    {
                        return Result.AccessDenied;
                    }

                    break;

                case FormType.MasterEditAgent:
                    if (model.TargetRoleId != RoleType.Agent)
                    {
                        return Result.AccessDenied;
                    }

                    break;

                case FormType.SuperEditMember:
                    if (model.TargetRoleId != RoleType.Member)
                    {
                        return Result.AccessDenied;
                    }

                    break;

                case FormType.MasterEditMember:
                    if (model.TargetRoleId != RoleType.Member)
                    {
                        return Result.AccessDenied;
                    }

                    break;

                case FormType.AgentEditMember:
                    if (model.TargetRoleId != RoleType.Member)
                    {
                        return Result.AccessDenied;
                    }

                    break;

                default:
                    return Result.AccessDenied;
            }

            return Result.Success;
        }

        private Result ValidateDownline(BaseModel model)
        {
            return Result.Success;
        }

        /// <summary>
        /// Validate the product profile.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns model validation.</returns>
        private Result ValidateProductProfile(UpdateModel model)
        {
            switch (model.SettingType)
            {
                case SettingType.All:
                    if ((model.PositionTakings == null || model.PositionTakings.Count() == 0)
                        && (model.BetLimits == null || model.BetLimits.Count() == 0)
                        && (model.Discounts == null || model.Discounts.Count() == 0)
                        && (model.Statuses == null || model.Statuses.Count() == 0))
                    {
                        return Result.NoChange;
                    }

                    break;

                case SettingType.PositionTaking:
                    if (model.PositionTakings == null || model.PositionTakings.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    break;

                case SettingType.BetLimit:
                    if (model.BetLimits == null || model.BetLimits.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    break;

                case SettingType.Discount:
                    if (model.Discounts == null || model.Discounts.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    break;

                case SettingType.Status:
                    if (model.Statuses == null || model.Statuses.Count() == 0)
                    {
                        return Result.NoChange;
                    }

                    break;
            }

            return Result.Success;
        }
    }
}