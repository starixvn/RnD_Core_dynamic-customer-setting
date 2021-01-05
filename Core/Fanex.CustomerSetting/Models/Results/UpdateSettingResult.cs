namespace Fanex.CustomerSetting.Models.Results
{
    using Fanex.CustomerSetting.Models.Constants;
    using System.Collections.Generic;
    using System.Linq;

    public class UpdateSettingResult : Result
    {
        public UpdateSettingResult(IEnumerable<SettingResult> subResults)
            : base(0)
        {
            SubResults = subResults.ToList();
        }

        public UpdateSettingResult(IEnumerable<IResult> results)
            : base(0)
        {
            if (results != null)
            {
                var subResults = new List<SettingResult>();

                foreach (IResult result in results)
                {

                    if (result is UpdateSettingResult updateSettingResult)
                    {
                        subResults.AddRange(updateSettingResult.SubResults);
                    }
                    else
                    {

                        subResults.Add(result is SettingResult settingResult ? settingResult : new SettingResult(result));
                    }
                }

                SubResults = subResults;
            }
        }

        public UpdateSettingResult(IResult result)
            : base(result.Code, result.Message)
        {
            SubResults = new List<SettingResult> { new SettingResult(result) };
        }

        public List<SettingResult> SubResults { get; private set; }

        public void AddSubResult(IResult result)
        {
            var settingResult = result as SettingResult;

            SubResults.Add(settingResult ?? new SettingResult(result));
        }

        public void RemoveNoChangeByUMSubResults() => RemoveSubResultByCode(ResultValues.SuccessButDoNothingBecauseUM);


        public override bool IsSuccessful
        {
            get
            {
                if (SubResults != null && SubResults.Any())
                {
                    return SubResults.FirstOrDefault(x => !x.IsSuccessful) == null;
                }

                return base.IsSuccessful;
            }
        }
        private void RemoveSubResultByCode(int code) => SubResults = SubResults.Where(m => m.Code != code).ToList();
    }
}