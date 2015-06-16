namespace Fanex.CustomerSetting.Core.Results.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    public class MultipleResults : IMultipleResults
    {
        public MultipleResults(IEnumerable<IResults> resultsList)
        {
            ResultsList = resultsList;
        }

        public IEnumerable<IResults> ResultsList { get; private set; }

        public bool IsSuccessful
        {
            get
            {
                return ResultsList.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
            }
        }
    }
}