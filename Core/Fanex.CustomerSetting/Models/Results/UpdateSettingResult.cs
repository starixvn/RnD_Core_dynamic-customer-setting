namespace Fanex.CustomerSetting.Models.Results
{
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
                    var updateSettingResult = result as UpdateSettingResult;

                    if (updateSettingResult != null)
                    {
                        subResults.AddRange(updateSettingResult.SubResults);
                    }
                    else
                    {
                        var settingResult = result as SettingResult;

                        if (settingResult != null)
                        {
                            subResults.Add(settingResult);
                        }
                        else
                        {
                            subResults.Add(new SettingResult(result));
                        }
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

            if (settingResult == null)
            {
                SubResults.Add(new SettingResult(result));
            }
            else
            {
                SubResults.Add(settingResult);
            }
        }

        public void RemoveSubResultByCode(int code)
        {
            SubResults = SubResults.Where(m => m.Code != code).ToList();
        }

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
    }
}