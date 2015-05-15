namespace Fanex.CustomerSetting.Messages
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Services;

    public class MessageBuilderFactory : IMessageBuilderFactory
    {
        private readonly IResourceService _resourceService;

        public MessageBuilderFactory()
            : this(null)
        {
        }

        public MessageBuilderFactory(IResourceService resourceService)
        {
            _resourceService = resourceService ?? new ResourceService();
        }

        public IMessageBuilder Create(MsgType msgType)
        {
            switch (msgType)
            {
                case MsgType.Simple:
                    return new SimpleMessageBuilder();

                case MsgType.Status:
                case MsgType.Discount:
                case MsgType.BetLimit:
                    return new TemplateMessageBuilder<NormalMessage, SimpleRow>(_resourceService);

                case MsgType.PositionTaking:
                    return new TemplateMessageBuilder<PositionTakingMessage, PositionTakingRow>(_resourceService);

                case MsgType.PositionTakingDownline:
                    return new TemplateMessageBuilder<PositionTakingDownlineMessage, PositionTakingDownlineRow>(_resourceService);

                case MsgType.PositionTakingMin:
                    return new TemplateMessageBuilder<PositionTakingMinMessage, PositionTakingMinRow>(_resourceService);

                case MsgType.PositionTakingMax:
                    return new TemplateMessageBuilder<PositionTakingMaxMessage, PositionTakingMaxRow>(_resourceService);

                case MsgType.PositionTakingMinMax:
                    return new TemplateMessageBuilder<PositionTakingMinMaxMessage, PositionTakingMinMaxRow>(_resourceService);

                default:
                    return new NormalMessageBuilder();
            }
        }
    }
}