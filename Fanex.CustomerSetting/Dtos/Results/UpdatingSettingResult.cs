namespace Fanex.CustomerSetting.Dtos.Results
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Constants;

    public class UpdatingSettingResult : Result
    {
        public UpdatingSettingResult(IEnumerable<SettingResult> subResults)
            : base(0)
        {
            SubResults = subResults;
        }

        public UpdatingSettingResult(IResult result)
            : base(result.Code, result.Message)
        {
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