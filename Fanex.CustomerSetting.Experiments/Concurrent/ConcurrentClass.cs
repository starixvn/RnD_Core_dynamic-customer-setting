namespace Fanex.CustomerSetting.Experiments.Concurrent
{
    using System;
    using System.Threading;

    public class ConcurrentClass
    {
        public void Run(string callerName)
        {
            Console.WriteLine("{0} begins calling concurrent class.", callerName);

            Thread.Sleep(5 * 1000);

            Console.WriteLine("{0} ends calling concurrent class.", callerName);
        }
    }
}