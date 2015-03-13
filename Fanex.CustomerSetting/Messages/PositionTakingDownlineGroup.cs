namespace Fanex.CustomerSetting.Messages
{
    using System.Collections.Generic;

    /// <summary>
    /// PositionTakingDownlineGroup class.
    /// </summary>
    public class PositionTakingDownlineGroup : PositionTakingRow
    {
        public IEnumerable<PositionTakingDownlineRow> Rows { get; set; }
    }
}