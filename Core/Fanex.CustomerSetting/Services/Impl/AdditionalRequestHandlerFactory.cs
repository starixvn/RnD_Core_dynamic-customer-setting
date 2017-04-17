namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Common.Helper;

    internal static class AdditionalRequestHandlerFactory
    {
        private static ConcurrentDictionary<short, IAdditionalRequestHandler> cachedHandlers = new ConcurrentDictionary<short, IAdditionalRequestHandler>();
        private static IEnumerable<Type> requestHanlersInAssemblies;

        public static IAdditionalRequestHandler GetInstance(short settingTypeId)
        {
            Check.Argument.IsNotNegativeOrZero(settingTypeId, nameof(settingTypeId));

            IAdditionalRequestHandler matchingHandler = GetInstanceFromCache(settingTypeId) ?? CreateInstanceAndCache(settingTypeId);

            return matchingHandler;
        }

        private static IAdditionalRequestHandler GetInstanceFromCache(short settingTypeId)
        {
            IAdditionalRequestHandler handler = null;

            cachedHandlers.TryGetValue(settingTypeId, out handler);

            return handler;
        }

        private static IAdditionalRequestHandler CreateInstanceAndCache(short settingTypeId)
        {
            IAdditionalRequestHandler handler = null;
            string handlerNamespace = $"Fanex.CustomerSetting.SettingType{settingTypeId}.Services";
            Type handlerType = requestHanlersInAssemblies?.FirstOrDefault(type => type.Namespace.Contains(handlerNamespace));

            if (handlerType == null)
            {
                // There is a case that some assemblies was loaded after the first run of DetechRequestHanlersInAssemblies, we can't fetch types in those assemblies.
                // So we try to get loaded assemblies one more time to reduce the risk.
                requestHanlersInAssemblies = DetechRequestHanlersInAssemblies();
                handlerType = requestHanlersInAssemblies.FirstOrDefault(type => type.Namespace.Contains(handlerNamespace));

                // There is a case that the route request is handled by another application, which has not loaded the DLL yet.
                // We spend the resource to load the missing DLL one more time to give it a try.
                if (handlerType == null)
                {
                    Assembly loadedAssembly = LoadAssembly(settingTypeId);
                    IEnumerable<Type> handlers = FilterAdditionalRequestHandlerTypes(GetTypesInAssembly(loadedAssembly));

                    handlerType = handlers.FirstOrDefault();
                }

                if (handlerType == null)
                {
                    throw new DllNotFoundException($"{handlerNamespace} namespace is not found.");
                }
            }

            handler = Activator.CreateInstance(handlerType) as IAdditionalRequestHandler;

            cachedHandlers.TryAdd(settingTypeId, handler);

            return handler;
        }

        private static Assembly LoadAssembly(short settingTypeId)
        {
            string assemblyFilename = $"Fanex.CustomerSetting.SettingType{settingTypeId}.dll";
            string assemblyLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", assemblyFilename);
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyLocation);

            return Assembly.Load(assemblyName);
        }

        private static IEnumerable<Type> DetechRequestHanlersInAssemblies()
        {
            IEnumerable<Type> types = Type.EmptyTypes;
            ICollection assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                types = types.Concat(GetTypesInAssembly(assembly));
            }

            return FilterAdditionalRequestHandlerTypes(types);
        }

        private static IEnumerable<Type> GetTypesInAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types;
            }
        }

        private static IEnumerable<Type> FilterAdditionalRequestHandlerTypes(IEnumerable<Type> types)
            => types.Where(type => TypeIsPublicClass(type) && IsAdditionalRequestHandler(type));

        private static bool TypeIsPublicClass(Type type)
        {
            return type != null && type.IsPublic && type.IsClass && !type.IsAbstract;
        }

        private static bool IsAdditionalRequestHandler(Type type)
        {
            return typeof(IAdditionalRequestHandler).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}