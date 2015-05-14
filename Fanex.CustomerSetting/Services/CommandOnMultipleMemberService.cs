namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.CommandTypes;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Utility;

    public class CommandOnMultipleMemberService : ICommandOnMultipleCustomerService
    {
        private IQueryOnMembersService _queryService;

        public CommandOnMultipleMemberService()
            : this(null)
        {
        }

        public CommandOnMultipleMemberService(IQueryOnMembersService queryService)
        {
            _queryService = queryService ?? new QueryOnMembersService();
        }

        public IList<CommandResult> Do(MultipleCommandInfo multipleCommandInfo)
        {
            var memberGroups = _queryService.SelectMemberGroup(multipleCommandInfo.TargetCustIds);
            var groupedMemberIds = memberGroups.Where(x => x.IsGrouped).Select(x => x.CustId).ToList();
            IList<CommandResult> groupedMembersCommandResults = new List<CommandResult>();
            IList<CommandResult> ungroupedMembersCommandResults = new List<CommandResult>();
            if (groupedMemberIds.Count() > 0)
            {
                groupedMembersCommandResults = DoGroupedMembers(multipleCommandInfo, groupedMemberIds);
            }

            var ungroupedMemberIds = memberGroups.Where(x => !x.IsGrouped).Select(x => x.CustId).ToList();
            if (ungroupedMemberIds.Count() > 0)
            {
                ungroupedMembersCommandResults = DoUngroupedMembers(multipleCommandInfo, ungroupedMemberIds);
            }

            return ArrangeCommandResults(multipleCommandInfo.TargetCustIds, groupedMemberIds, groupedMembersCommandResults, ungroupedMemberIds, ungroupedMembersCommandResults);
        }

        private IList<CommandResult> DoGroupedMembers(MultipleCommandInfo commandsInfo, IList<int> memberIds)
        {
            var commandResults = new List<CommandResult>();
            var groupedMembersCommandsInfo = commandsInfo.Clone<MultipleCommandInfo>();
            groupedMembersCommandsInfo.TargetCustIds = memberIds;
            var hasPositionTakingCommand = groupedMembersCommandsInfo.CommandType == CommandType.All
                || groupedMembersCommandsInfo.CommandType == CommandType.PositionTaking;
            CommandResult positionTakingMultipleCommandResult = null;
            if (hasPositionTakingCommand)
            {
                IMultipleCommand positionTakingMultipleCommand = new PositionTakingMultipleCommand();
                positionTakingMultipleCommandResult = positionTakingMultipleCommand.Do(groupedMembersCommandsInfo);
            }

            while (true)
            {
                var commandInfo = groupedMembersCommandsInfo.Take();
                if (commandInfo == null)
                {
                    break;
                }

                CommandResult commandResult = null;
                if (commandInfo.CommandType == CommandType.All)
                {
                    ICommand command = new AllNotPositionTakingCommand();
                    var allNotPositionTakingCommandResult = command.Do(commandInfo);
                    commandResult = new CommandResult(allNotPositionTakingCommandResult.StatusResult, allNotPositionTakingCommandResult.DiscountResult, allNotPositionTakingCommandResult.BetLimitResult, positionTakingMultipleCommandResult);
                }
                else if (commandInfo.CommandType == CommandType.PositionTaking)
                {
                    commandResult = positionTakingMultipleCommandResult;
                }
                else
                {
                    ICommand command = CommandFactory.Get(commandInfo.CommandType);
                    commandResult = command.Do(commandInfo);
                }

                commandResults.Add(commandResult);
            }

            return commandResults;
        }

        private IList<CommandResult> DoUngroupedMembers(MultipleCommandInfo commandsInfo, IList<int> memberIds)
        {
            var commandResults = new List<CommandResult>();
            var ungroupedMembersCommandsInfo = commandsInfo.Clone<MultipleCommandInfo>();
            ungroupedMembersCommandsInfo.TargetCustIds = memberIds;
            while (true)
            {
                var commandInfo = ungroupedMembersCommandsInfo.Take();
                if (commandInfo == null)
                {
                    break;
                }

                ICommand command = CommandFactory.Get(commandInfo.CommandType);
                var commandResult = command.Do(commandInfo);
                commandResults.Add(commandResult);
            }

            return commandResults;
        }

        private IList<CommandResult> ArrangeCommandResults(IEnumerable<int> memberIds, IList<int> groupedMemberIds, IList<CommandResult> groupedResults, IList<int> ungroupedMemberIds, IList<CommandResult> ungroupedResults)
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
                    commandResult = groupedResults[id];
                }

                commandResults.Add(commandResult);
            }

            return commandResults;
        }
    }
}