namespace Fanex.CustomerSetting.Core.Dtos.Results.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    public class Results : IResults
    {
        public Results(int id, string name, IEnumerable<IResult> results)
        {
            Id = id;
            Name = name;
            ResultList = results;
        }

        public IEnumerable<IResult> ResultList { get; private set; }

        public string Name { get; private set; }

        public int Id { get; private set; }

        public virtual bool IsSuccessful
        {
            get
            {
                return ResultList.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
            }
        }
    }
}