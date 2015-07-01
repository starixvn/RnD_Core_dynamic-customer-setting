namespace Fanex.CustomerSetting.Dtos.Results
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Constants;

    public class UpdatingSettingResult : Result
    {
        public static readonly UpdatingSettingResult SuccessUpdatingSettingResult = new UpdatingSettingResult(null, ResultValue.Success);

        public UpdatingSettingResult(IEnumerable<SettingResult> subResults, int code = 0)
            : base(code)
        {
            SubResults = subResults;
        }

        public IEnumerable<SettingResult> SubResults { get; private set; }

        public override bool IsSuccessful
        {
            get
            {
                if (SubResults != null && SubResults.Any())
                {
                    return SubResults.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
                }

                return base.IsSuccessful;
            }
        }
    }
}