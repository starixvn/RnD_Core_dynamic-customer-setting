namespace Fanex.CustomerSetting.Experiments.Concurrent
{
    using System.Collections.Concurrent;

    public class ConcurrentExperiment
    {
        private const string CacheKey = "ConcurrentClass";

        private static ConcurrentDictionary<string, ConcurrentClass> settingTypeServiceInstances = new ConcurrentDictionary<string, ConcurrentClass>();

        public void Init()
        {
            settingTypeServiceInstances.TryAdd(CacheKey, new ConcurrentClass());
        }
        
        public ConcurrentClass GetConcurrentClass()
        {
            ConcurrentClass concurrentClass = null;

            settingTypeServiceInstances.TryGetValue(CacheKey, out concurrentClass);

            return concurrentClass;
        }
    }
}