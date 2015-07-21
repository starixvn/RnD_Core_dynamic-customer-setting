namespace Fanex.CustomerSetting.Experiments
{
    using System;
    using System.Threading.Tasks;
    using Fanex.CustomerSetting.Experiments.Concurrent;

    internal class Program
    {
        private static void Main(string[] args)
        {
            RunConcurrentExperiment();
        }

        private static void RunConcurrentExperiment()
        {
            Task.Factory.StartNew(() =>
            {
                //// Caller A
                var experiment = new ConcurrentExperiment();
                experiment.Init();

                var concurrentClassA = experiment.GetConcurrentClass();
                concurrentClassA.Run("Caller A");
            });

            Task.Factory.StartNew(() =>
            {
                //// Caller B
                var experiment = new ConcurrentExperiment();
                experiment.Init();

                var concurrentClassB = experiment.GetConcurrentClass();
                concurrentClassB.Run("Caller B");
            });

            Console.ReadLine();
        }
    }
}