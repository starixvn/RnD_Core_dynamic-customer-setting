namespace Fanex.CustomerSetting.CommandTypes
{
    using System;
    using Fanex.CustomerSetting.Enumerations;

    public class CommandFactory : ICommandFactory
    {
        public ICommand Create(CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.All:
                    return new AllCommand();

                case CommandType.Status:
                    return new StatusCommand();

                case CommandType.Discount:
                    return new DiscountCommand();

                case CommandType.BetLimit:
                    return new BetLimitCommand();

                case CommandType.PositionTaking:
                    return new PositionTakingCommand();

                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }
    }
}