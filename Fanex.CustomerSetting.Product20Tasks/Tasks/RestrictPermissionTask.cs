namespace Fanex.CustomerSetting.Product20Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.PlugableTasks;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Product20Tasks.Constants;
    using Fanex.CustomerSetting.Product20Tasks.Criteria;
    using Fanex.CustomerSetting.Product20Tasks.Models;
    using Fanex.CustomerSetting.Product20Tasks.Resources;
    using Fanex.Data.Repository;

    public class RestrictPermissionTask : IPostQueryCustomerSettingTask, IPreUpdateCustomerSettingTask, IPreUpdateMultipleCustomersSettingTask
    {
        private readonly IDynamicRepository _repository;

        public RestrictPermissionTask()
            : this(null)
        {
        }

        public RestrictPermissionTask(IDynamicRepository repository)
        {
            _repository = repository ?? new DynamicRepository();
        }

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, Setting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(setting, nameof(setting));

            var criteria = new SettingRestrictionModesCriteria
            {
                ProductId = query.ProductId,
                SystemId = settingContext.System,
                TargetId = query.CustomerId,
                TargetRoleId = query.CustomerLevel.Value,
                ActorRoleId = settingContext.Actor.Level.Value
            };
            IEnumerable<SettingTypeRestrictionMode> settingTypeRestrictionModes = GetSettingTypeRestrictionModes(criteria);

            if (settingTypeRestrictionModes?.Any() == true && setting?.SettingTypes?.Any() == true)
            {
                var restrictedSettingTypeNames = new List<string>();

                foreach (SettingTypeRestrictionMode restrictionMode in settingTypeRestrictionModes)
                {
                    SettingType matchedSettingType = setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == restrictionMode.SettingTypeId);

                    if (matchedSettingType != null)
                    {
                        matchedSettingType.DisplayMode = Math.Max(matchedSettingType.DisplayMode, restrictionMode.Mode);

                        if (restrictionMode.Mode == RestrictionModes.ReadOnly)
                        {
                            restrictedSettingTypeNames.Add(matchedSettingType.Name);
                        }
                    }
                }

                if (restrictedSettingTypeNames.Any())
                {
                    string warningMessage = Message.ResourceManager.GetString($"Msg_{ErrorCodes.RunningGameUpdateRestrictionError}");

                    setting.WarningMessage
                        = new Result(ErrorCodes.RunningGameUpdateRestrictionError, $"{warningMessage}: {string.Join(", ", restrictedSettingTypeNames)}.");
                }

                setting.SettingTypes = setting.SettingTypes.Where(settingType => settingType.DisplayMode != RestrictionModes.Hidden);
            }
        }

        public void Invoke(ISettingContext settingContext, UpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(command, nameof(command));
            FilterUpdatingSettingTypes(settingContext, command);
        }

        public void Invoke(ISettingContext settingContext, MultipleUpdateCustomerSettingCommand command)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(command, nameof(command));
            FilterUpdatingSettingTypes(settingContext, command);
        }

        private void FilterUpdatingSettingTypes(ISettingContext settingContext, UpdateCustomerSettingCommand command)
        {
            var criteria = new SettingRestrictionModesCriteria
            {
                ProductId = command.Setting.ProductId,
                SystemId = settingContext.System,
                TargetId = command.Setting.CustomerId,
                TargetRoleId = command.Setting.CustomerLevel.Value,
                ActorRoleId = settingContext.Actor.Level.Value
            };
            IEnumerable<SettingTypeRestrictionMode> settingTypeRestrictionModes = GetSettingTypeRestrictionModes(criteria);

            if (settingTypeRestrictionModes?.Any() == true && command?.Setting?.SettingTypes?.Any() == true)
            {
                foreach (SettingTypeRestrictionMode restrictionMode in settingTypeRestrictionModes)
                {
                    SettingType matchedSettingType = command.Setting.SettingTypes.FirstOrDefault(
                        settingType => settingType.Id == restrictionMode.SettingTypeId);

                    if (matchedSettingType != null)
                    {
                        matchedSettingType.DisplayMode = Math.Max(matchedSettingType.DisplayMode, restrictionMode.Mode);
                    }
                }

                command.Setting.SettingTypes = command.Setting.SettingTypes.Where(settingType => settingType.DisplayMode == RestrictionModes.Editable);
            }
        }

        private IEnumerable<SettingTypeRestrictionMode> GetSettingTypeRestrictionModes(SettingRestrictionModesCriteria criteria)
        {
            return _repository.Fetch<SettingTypeRestrictionMode>(criteria);
        }
    }
}