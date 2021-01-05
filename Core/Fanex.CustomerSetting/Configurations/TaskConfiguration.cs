namespace Fanex.CustomerSetting.Configurations
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using Tasks;

    internal static class TaskConfiguration
    {
        private static IEnumerable<SiteProductTasks> configuredTasks
            = ReadTasksFromConfigurationFiles(CustomerSettingConfiguration.Configurations.TasksConfigurationPath);

        private static ConcurrentDictionary<string, object> cachedTasks = new ConcurrentDictionary<string, object>();

        public static void Initialize(string configurationPath)
        {
            configurationPath = string.IsNullOrEmpty(configurationPath)
                ? CustomerSettingConfiguration.Configurations.TasksConfigurationPath
                : configurationPath;

            configuredTasks = ReadTasksFromConfigurationFiles(configurationPath);
            cachedTasks = new ConcurrentDictionary<string, object>();
        }

        public static IEnumerable<T> PickTasks<T>(byte systemId, byte productId) where T : class
        {
            return PickTasks<T>(systemId, productId, siteProductTasks => siteProductTasks.Tasks);
        }

        public static IEnumerable<T> PickTasks<T>(byte systemId, byte productId, IEnumerable<short> settingTypeIds) where T : class
        {
            return PickTasks<T>(systemId, productId, siteProductTasks => PickSettingTypesTaskNames(siteProductTasks, settingTypeIds));
        }

        public static IEnumerable<T> PickTasks<T>(byte systemId, byte productId, short settingTypeId) where T : class
        {
            return PickTasks<T>(systemId, productId, siteProductTasks => PickSettingTypeTaskNames(siteProductTasks, settingTypeId));
        }

        private static IEnumerable<T> PickTasks<T>(
            byte systemId,
            byte productId,
            Func<SiteProductTasks, IEnumerable<SettingTypeTasks>> filter) where T : class
        {
            IEnumerable<T> runnableTasks = Enumerable.Empty<T>();

            if (configuredTasks.Any())
            {
                runnableTasks = configuredTasks
                    .Where(siteProductTasks => IsMatchedSiteProductTasks(siteProductTasks, systemId, productId))
                    .OrderBy(siteProductTasks => siteProductTasks.Order)
                    .SelectMany(siteProductTasks => filter(siteProductTasks).OrderBy(settingTypeTasks => settingTypeTasks.Order))
                    .SelectMany(settingTypeTask => settingTypeTask.TaskNames)
                    .Distinct()
                    .Select(taskName => InjectTask<T>(taskName))
                    .Where(task => task != null);
            }

            return runnableTasks;
        }

        private static bool IsMatchedSiteProductTasks(SiteProductTasks taskList, byte systemId, byte productId)
        {
            return (taskList.System == null || taskList.System == systemId)
                && (taskList.ProductId == null || taskList.ProductId == productId);
        }

        private static IEnumerable<SettingTypeTasks> PickSettingTypesTaskNames(SiteProductTasks productTask, IEnumerable<short> settingTypeIds)
        {
            return productTask.Tasks
                .Where(settingTypeTasks => settingTypeTasks.SettingTypeId == null || settingTypeIds.Any(settingTypeId => settingTypeId == settingTypeTasks.SettingTypeId));
        }

        private static IEnumerable<SettingTypeTasks> PickSettingTypeTaskNames(SiteProductTasks productTask, short settingTypeId)
        {
            return productTask.Tasks
                .Where(settingTypeTasks => settingTypeTasks.SettingTypeId == null || settingTypeTasks.SettingTypeId == settingTypeId);
        }

        private static T InjectTask<T>(string taskName) where T : class
        {
            T task = default(T);

            if (!string.IsNullOrEmpty(taskName))
            {
                task = GetTaskFromCache<T>(taskName);
                task = task ?? CreateAndCacheTask<T>(taskName);
            }

            return task;
        }

        private static T CreateAndCacheTask<T>(string taskName) where T : class
        {
            Type taskType = Type.GetType(taskName);
            T task = default(T);

            if (typeof(T).IsAssignableFrom(taskType))
            {
                task = Activator.CreateInstance(taskType) as T;
                cachedTasks.TryAdd(taskName, task);
            }

            return task;
        }

        private static T GetTaskFromCache<T>(string taskName) where T : class
        {
            T task = default(T);
            object cachedTask = null;

            cachedTasks.TryGetValue(taskName, out cachedTask);

            if (cachedTask is T)
            {
                task = cachedTasks as T;
            }

            return task;
        }

        private static IEnumerable<SiteProductTasks> ReadTasksFromConfigurationFiles(string path)
        {
            var tasks = new List<SiteProductTasks>();
            IEnumerable<string> filePaths = DetechFilePaths(path);

            foreach (string filePath in filePaths)
            {
                var newTasks = JsonConvert.DeserializeObject<IEnumerable<SiteProductTasks>>(File.ReadAllText(filePath));

                tasks.AddRange(newTasks);
            }

            return tasks;
        }

        private static IEnumerable<string> DetechFilePaths(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            VerifyPath(path);

            if (IsDirectory(path))
            {
                IEnumerable<string> filePaths = Directory.GetFiles(path);

                filePaths = filePaths.Where(filePath => Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase));

                return filePaths;
            }

            return new[] { path };
        }

        private static void VerifyPath(string path)
        {
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                throw new DirectoryNotFoundException($"The path to the configuration file of pluggable tasks, which is {path}, is invalid");
            }
        }

        private static bool IsDirectory(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}