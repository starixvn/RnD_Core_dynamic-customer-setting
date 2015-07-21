namespace Fanex.CustomerSetting.Experiments.Concurrent
{
    using System;
    using System.Threading;

    public class SettingTypeService
    {
        public void UpdateSettingType(string userName)
        {
            Console.WriteLine("{0} starts update setting type at {1}.", userName, DateTime.Now.ToLongTimeString());

            Thread.Sleep(5 * 1000);

            Console.WriteLine("{0} ends update setting type at {1}.", userName, DateTime.Now.ToLongTimeString());
        }
    }
}