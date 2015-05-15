namespace Fanex.CustomerSetting.Messages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resources;
    using Fanex.CustomerSetting.Services;

    public class TemplateMessageBuilder<TMessage, TRow> : BaseMessageBuilder, IMessageBuilder
        where TMessage : TemplateMessage<TRow>
        where TRow : SimpleRow
    {
        private readonly IResourceService _resourceService;

        public TemplateMessageBuilder(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public string Build(MessageInfo messageInfo)
        {
            var templateMessage = Deserialize<TMessage>(messageInfo.ErrMsg);
            var message = TranslateResource(templateMessage.MessageId, messageInfo.SettingType);
            var stringBuilder = new StringBuilder(message);
            if (messageInfo.SettingLevel != SettingLevel.Product)
            {
                if (typeof(TRow) == typeof(PositionTakingDownlineRow))
                {
                    var downlineRows = templateMessage.Rows.Cast<PositionTakingDownlineRow>();
                    stringBuilder.Append(TranslatePositionTakingDownlineRows(templateMessage.MessageId, downlineRows, messageInfo));
                }
                else
                {
                    stringBuilder.Append("<div class='rows'>");
                    foreach (var row in templateMessage.Rows)
                    {
                        stringBuilder.AppendFormat("<div class='row'>{0}</div>", row.ToString(_resourceService, messageInfo));
                    }

                    stringBuilder.Append("</div>");
                }
            }

            return stringBuilder.ToString();
        }

        private string TranslatePositionTakingDownlineRows(
            int messageId,
            IEnumerable<PositionTakingDownlineRow> rows,
            MessageInfo messageInfo)
        {
            var groups = rows.GroupBy(row => new
            {
                row.SportType,
                row.BetType,
                row.IsLive
            }).Select(group => new PositionTakingDownlineGroup
            {
                SportType = group.Key.SportType,
                BetType = group.Key.BetType,
                IsLive = group.Key.IsLive,
                Rows = group.ToList()
            });

            var stringBuilder = new StringBuilder("<div class='rows'>");
            foreach (var group in groups)
            {
                var roleTypeName = GetRoleTypeName(messageId);
                var userNames = string.Join(", ", group.Rows.Select(row => row.UserName));
                stringBuilder.AppendFormat("<div class='row'>{0} - {1}: {2}</div>", group.ToString(_resourceService, messageInfo), roleTypeName, userNames);
            }

            stringBuilder.Append("</div>");
            return stringBuilder.ToString();
        }

        private string GetRoleTypeName(int messageId)
        {
            switch ((MsgCode)messageId)
            {
                case MsgCode.PositionTakingMastersExceedSuper:
                case MsgCode.PositionTakingMastersExceedMaster:
                    return Lang.Masters;

                case MsgCode.PositionTakingAgentsExceedMaster:
                case MsgCode.PositionTakingAgentsExceedAgent:
                    return Lang.Agents;

                case MsgCode.PositionTakingMembersExceedSuper:
                case MsgCode.PositionTakingMembersExceedMaster:
                case MsgCode.PositionTakingMembersExceedAgent:
                    return Lang.Members;

                default:
                    return string.Empty;
            }
        }
    }
}