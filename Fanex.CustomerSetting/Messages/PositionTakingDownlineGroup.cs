namespace Fanex.CustomerSetting.Messages
{
    using System.Collections.Generic;

    public class PositionTakingDownlineGroup : PositionTakingRow
    {
        public IEnumerable<PositionTakingDownlineRow> Rows { get; set; }
    }
}