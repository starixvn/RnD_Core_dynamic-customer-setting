namespace Fanex.CustomerSetting.Dtos.Results
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Constants;

    public class UpdateSettingResult : Result
    {
        public UpdateSettingResult(IEnumerable<SettingResult> subResults)
            : base(0)
        {
            SubResults = subResults;
        }

        public UpdateSettingResult(IResult result)
            : base(result.Code, result.Message)
        {
            SubResults = new List<SettingResult> { new SettingResult(result) };
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