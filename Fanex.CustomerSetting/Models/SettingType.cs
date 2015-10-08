namespace Fanex.CustomerSetting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Models.Results;

    [Serializable]
    public class SettingType
    {
        private const string SettingTypeXmlFormat = "<ROOT>{0}</ROOT>";
        private IEnumerable<ISettingItem> _settingItems;

        public short Id { get; set; }

        public string Name { get; set; }

        public byte SettingLevel { get; set; }

        public byte DisplayOrder { get; set; }

        public byte DisplayMode { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsError { get; set; }

        public IEnumerable<ISettingItem> SettingItems
        {
            get
            {
                if (_settingItems == null)
                {
                    _settingItems = new List<ISettingItem>();
                }

                return _settingItems;
            }

            set
            {
                _settingItems = value;
            }
        }

        /// <summary>
        /// For Controller create exactly concrete class when get json from client.
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.GetType().AssemblyQualifiedName;
            }
        }

        public override string ToString()
        {
            return string.Join(base.ToString(), this.Id);
        }

        public virtual string ToXml()
        {
            var settingItemsXml = string.Join(string.Empty, SettingItems.Select(settingItem => settingItem.ToXml()));

            return string.Format(SettingTypeXmlFormat, settingItemsXml);
        }

        public IResult Validate()
        {
            if (!SettingItems.Any())
            {
                return Result.NoChange;
            }

            var failedResult = GetFailedResult();

            return (failedResult == null) ? Result.Success : failedResult;
        }

        private IResult GetFailedResult()
        {
            foreach (var settingItem in SettingItems)
            {
                var result = settingItem.Validate();

                if (result.IsNotOk)
                {
                    return result;
                }
            }

            return null;
        }
    }
}