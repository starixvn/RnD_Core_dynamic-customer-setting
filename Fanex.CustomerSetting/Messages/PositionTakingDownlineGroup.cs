namespace Fanex.CustomerSetting.Messages
{
    using System.Collections.Generic;

    /// <summary>
    /// PositionTakingDownLineGroup class.
    /// </summary>
    public class PositionTakingDownlineGroup : PositionTakingRow
    {
        public IEnumerable<PositionTakingDownlineRow> Rows { get; set; }
    }
}