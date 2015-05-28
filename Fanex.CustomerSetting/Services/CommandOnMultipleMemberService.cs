namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.CommandTypes;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class CommandOnMultipleMemberService : ICommandOnMultipleCustomerService
    {
        private readonly IValidationService _validationService;
        private readonly IMemberGroupService _memberGroupService;
        private readonly CommandFactory _commandFactory;

        public CommandOnMultipleMemberService()
            : this(null, null, null)
        {
        }

        public CommandOnMultipleMemberService(
            IValidationService validationService,
            IMemberGroupService memberGroupService,
            CommandFactory commandFactory)
        {
            _validationService = validationService ?? new ValidationService();
            _memberGroupService = memberGroupService ?? new MemberGroupService();
            _commandFactory = commandFactory ?? new CommandFactory();
        }

        public IList<CommandResult> Execute(MultipleCommandInfo multipleCommandInfo)
        {
            Result validationResult = _validationService.Validate(multipleCommandInfo);
            if (!validationResult.IsSuccess)
            {
                var commandResult = new CommandResult(multipleCommandInfo, validationResult);
                return new List<CommandResult> { commandResult };
            }

            var memberGroups = _memberGroupService.SelectMemberGroup(multipleCommandInfo.TargetCustIds);
            var groupedMemberIds = memberGroups.Where(x => x.IsGrouped).Select(x => x.CustId).ToList();
            IList<CommandResult> groupedMembersCommandResults = new List<CommandResult>();
            IList<CommandResult> ungroupedMembersCommandResults = new List<CommandResult>();
            if (groupedMemberIds.Count() > 0)
            {
                var groupedMembersCommandsInfo = new MultipleCommandInfo(multipleCommandInfo);
                groupedMembersCommandsInfo.TargetCustIds = groupedMemberIds;
                groupedMembersCommandResults = DoGroupedMembers(groupedMembersCommandsInfo);
            }

            var ungroupedMemberIds = memberGroups.Where(x => !x.IsGrouped).Select(x => x.CustId).ToList();
            if (ungroupedMemberIds.Count() > 0)
            {
                var ungroupedMembersCommandsInfo = new MultipleCommandInfo(multipleCommandInfo);
                ungroupedMembersCommandsInfo.TargetCustIds = ungroupedMemberIds;
                ungroupedMembersCommandResults = DoUngroupedMembers(ungroupedMembersCommandsInfo);
            }

            return Join(multipleCommandInfo.TargetCustIds, groupedMemberIds, groupedMembersCommandResults, ungroupedMemberIds, ungroupedMembersCommandResults);
        }

        private IList<CommandResult> DoGroupedMembers(MultipleCommandInfo multipleCommandInfo)
        {
            var commandResults = new List<CommandResult>();
            var hasPositionTakingCommand = multipleCommandInfo.CommandType == CommandType.All
                || multipleCommandInfo.CommandType == CommandType.PositionTaking;
            CommandResult positionTakingMultipleCommandResult = null;
            if (hasPositionTakingCommand)
            {
                IMultipleCommand positionTakingMultipleCommand = new PositionTakingMultipleCommand();
                positionTakingMultipleCommandResult = positionTakingMultipleCommand.Execute(multipleCommandInfo);
            }

            while (true)
            {
                var commandInfo = multipleCommandInfo.Take();
                if (commandInfo == null)
                {
                    break;
                }

                CommandResult commandResult = null;
                if (commandInfo.CommandType == CommandType.All)
                {
                    ICommand command = new AllNotPositionTakingCommand();
                    var allNotPositionTakingCommandResult = command.Execute(commandInfo);
                    var settingResults = new List<SettingResult>
                    {
                        allNotPositionTakingCommandResult[SettingType.Status],
                        allNotPositionTakingCommandResult[SettingType.Discount],
                        allNotPositionTakingCommandResult[SettingType.BetLimit],
                        positionTakingMultipleCommandResult[SettingType.PositionTaking]
                    };

                    commandResult = new CommandResult(multipleCommandInfo, settingResults);
                }
                else if (commandInfo.CommandType == CommandType.PositionTaking)
                {
                    commandResult = positionTakingMultipleCommandResult;
                }
                else
                {
                    ICommand command = _commandFactory.Create(commandInfo.CommandType);
                    commandResult = command.Execute(commandInfo);
                }

                commandResults.Add(commandResult);
            }

            return commandResults;
        }

        private IList<CommandResult> DoUngroupedMembers(MultipleCommandInfo multipleCommandInfo)
        {
            var commandResults = new List<CommandResult>();
            while (true)
            {
                var commandInfo = multipleCommandInfo.Take();
                if (commandInfo == null)
                {
                    break;
                }

                ICommand command = _commandFactory.Create(commandInfo.CommandType);
                var commandResult = command.Execute(commandInfo);
                commandResults.Add(commandResult);
            }

            return commandResults;
        }

        private IList<CommandResult> Join(IEnumerable<int> memberIds, IList<int> groupedMemberIds, IList<CommandResult> groupedResults, IList<int> ungroupedMemberIds, IList<CommandResult> ungroupedResults)
        {
            var commandResults = new List<CommandResult>();
            foreach (var memberId in memberIds)
            {
                CommandResult commandResult = null;
                var id = groupedMemberIds.IndexOf(memberId);
                if (id != -1)
                {
                    commandResult = groupedResults[id];
                }
                else
                {
                    id = ungroupedMemberIds.IndexOf(memberId);
                    commandResult = ungroupedResults[id];
                }

                commandResults.Add(commandResult);
            }

            return commandResults;
        }
    }
}