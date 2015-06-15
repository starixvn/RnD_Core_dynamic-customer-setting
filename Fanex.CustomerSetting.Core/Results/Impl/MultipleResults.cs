namespace Fanex.CustomerSetting.Core.Results.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    public class MultipleResults : IMultipleResults
    {
        public MultipleResults()
        {
            ResultsList = new List<IResults>();
        }

        public List<IResults> ResultsList { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return ResultsList.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
            }
        }
    }
}