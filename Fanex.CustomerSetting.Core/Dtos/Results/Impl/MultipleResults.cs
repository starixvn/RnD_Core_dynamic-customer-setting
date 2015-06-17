namespace Fanex.CustomerSetting.Core.Dtos.Results.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    public class MultipleResults : IMultipleResults
    {
        public MultipleResults(IEnumerable<IResults> resultsList)
        {
            ResultsList = resultsList;
        }

        public static MultipleResults SuccessMultipleResults
        {
            get
            {
                return new MultipleResults(new List<Results>
                    {
                        Results.SuccessResults
                    });
            }
        }

        public bool IsSuccessful
        {
            get
            {
                return ResultsList.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
            }
        }

        public IEnumerable<IResults> ResultsList { get; private set; }
    }
}