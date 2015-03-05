namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;

    /// <summary>
    /// IXmlGenerator class.
    /// </summary>
    public interface IXmlGenerator
    {
        string Generate(IEnumerable<object> data);
    }
}