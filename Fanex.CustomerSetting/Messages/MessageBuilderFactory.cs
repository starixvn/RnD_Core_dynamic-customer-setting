namespace Fanex.CustomerSetting.Messages
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Services;

    public static class MessageBuilderFactory
    {
        public static IMessageBuilder Create(MsgType msgType, IResourceService resourceService)
        {
            switch (msgType)
            {
                case MsgType.Simple:
                    return new SimpleMessageBuilder();

                case MsgType.Status:
                case MsgType.Discount:
                case MsgType.BetLimit:
                    return new TemplateMessageBuilder<NormalMessage, SimpleRow>(resourceService);

                case MsgType.PositionTaking:
                    return new TemplateMessageBuilder<PositionTakingMessage, PositionTakingRow>(resourceService);

                case MsgType.PositionTakingDownline:
                    return new TemplateMessageBuilder<PositionTakingDownlineMessage, PositionTakingDownlineRow>(resourceService);

                case MsgType.PositionTakingMin:
                    return new TemplateMessageBuilder<PositionTakingMinMessage, PositionTakingMinRow>(resourceService);

                case MsgType.PositionTakingMax:
                    return new TemplateMessageBuilder<PositionTakingMaxMessage, PositionTakingMaxRow>(resourceService);

                case MsgType.PositionTakingMinMax:
                    return new TemplateMessageBuilder<PositionTakingMinMaxMessage, PositionTakingMinMaxRow>(resourceService);

                default:
                    return new NormalMessageBuilder();
            }
        }
    }
}