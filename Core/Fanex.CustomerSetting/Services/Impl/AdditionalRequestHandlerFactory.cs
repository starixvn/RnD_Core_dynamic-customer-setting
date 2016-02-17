namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fanex.Common.Helper;

    internal static class AdditionalRequestHandlerFactory
    {
        private static ConcurrentDictionary<short, IAdditionalRequestHandler> cachedHandlers = new ConcurrentDictionary<short, IAdditionalRequestHandler>();
        private static IEnumerable<Type> requestHanlersInAssemblies = DetechRequestHanlersInAssemblies();

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
            Type handlerType = requestHanlersInAssemblies.FirstOrDefault(type => type.Namespace.Contains(handlerNamespace));

            if (handlerType == null)
            {
                throw new DllNotFoundException($"{handlerNamespace} namespace is not found.");
            }

            handler = Activator.CreateInstance(handlerType) as IAdditionalRequestHandler;

            cachedHandlers.TryAdd(settingTypeId, handler);

            return handler;
        }

        private static IEnumerable<Type> DetechRequestHanlersInAssemblies()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            IEnumerable<Type> types = Type.EmptyTypes;
            IEnumerable<Type> matchingTypes = Type.EmptyTypes;
            ICollection assemblies = entryAssembly.GetReferencedAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] typesInAsm;

                try
                {
                    typesInAsm = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    typesInAsm = ex.Types;
                }

                types = types.Concat(typesInAsm);
            }

            matchingTypes = types.Where(type => TypeIsPublicClass(type) && IsAdditionalRequestHandler(type));

            return matchingTypes;
        }

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