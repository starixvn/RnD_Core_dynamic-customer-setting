namespace Fanex.CustomerSetting.CommandTypes
{
    using System;
    using Fanex.CustomerSetting.Enumerations;

    public class CommandFactory
    {
        public static ICommand Get(CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.All:
                    return new AllCommand();

                case CommandType.Status:
                    return new StatusCommand();

                case CommandType.Discount:
                    return new DiscountCommand();

                case CommandType.PositionTaking:
                    return new PositionTakingCommand();

                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }
    }
}