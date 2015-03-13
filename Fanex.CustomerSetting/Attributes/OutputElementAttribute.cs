namespace Fanex.CustomerSetting.Attributes
{
    using System;

    /// <summary>
    /// OutputElementAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OutputElementAttribute : Attribute
    {
        public OutputElementAttribute()
        {
        }

        public OutputElementAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}