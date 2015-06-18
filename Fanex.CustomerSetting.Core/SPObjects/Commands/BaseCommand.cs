namespace Fanex.CustomerSetting.Core.SPObjects.Commands
{
    using Fanex.NAP.Framework.Data;

    public class BaseCommand : CriteriaBase
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}