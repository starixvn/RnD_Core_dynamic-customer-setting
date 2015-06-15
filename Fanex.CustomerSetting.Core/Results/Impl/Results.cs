namespace Fanex.CustomerSetting.Core.Results.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    public class Results : IResults
    {
        public Results()
        {
            ResultList = new List<IResult>();
        }

        public IEnumerable<IResult> ResultList { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }

        public virtual bool IsSuccessful
        {
            get
            {
                return ResultList.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
            }
        }
    }
}