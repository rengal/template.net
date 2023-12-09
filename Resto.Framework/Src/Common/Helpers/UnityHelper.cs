using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class UnityHelper
    {
        private static string configurationFilePath;
    
        private static readonly IRuntimeProxyContainer FactoryContainer = new RuntimeProxyContainer("factoryContainer");
        public static IRuntimeProxyContainer GetFactoryContainer() => FactoryContainer;
    
        private static readonly IRuntimeProxyContainer DefaultContainer = new RuntimeProxyContainer("defaultContainer");
        public static IRuntimeProxyContainer GetDefaultContainer() => DefaultContainer;
    
        public static void SetConfigurationFile(string path)
        {
            configurationFilePath = path;
        }
    
        public interface IRuntimeProxyContainer : IUnityContainer
        {
            IRuntimeProxyContainer RegisterRuntime<TInterface>(string implementationTypeName);
        }
    
        private sealed class RuntimeProxyContainer : IRuntimeProxyContainer
        {
            private readonly object syncObj = new object();
            private readonly string containerName;
            private readonly ConcurrentDictionary<Type, Lazy<object>> runtimeInstances = new ConcurrentDictionary<Type, Lazy<object>>();
    
            [CanBeNull]
            private IUnityContainer underlyingContainer;
    
            public RuntimeProxyContainer(string containerName)
            {
                this.containerName = containerName;
            }
    
            public IRuntimeProxyContainer RegisterRuntime<TInterface>(string implementationTypeName)
            {
                var added = runtimeInstances.TryAdd(typeof(TInterface), new Lazy<object>(() => Activator.CreateInstance(Type.GetType(implementationTypeName, true))));
                if (!added)
                    throw new ArgumentException($"Instance of type {typeof(TInterface).Name} has already been registered");
    
                return this;
            }
    
            private IUnityContainer UnderlyingContainer
            {
                get
                {
                    if (underlyingContainer != null)
                        return underlyingContainer;
    
                    lock (syncObj)
                    {
                        if (underlyingContainer == null)
                        {
                            var section = GetUnityConfigurationSection();
                            if (section == null)
                                throw new InvalidOperationException("No unity section defined");
    
                            underlyingContainer = section.Configure(new UnityContainer(), containerName);
                        }
    
                        return underlyingContainer;
                    }
                }
            }
    
            private static UnityConfigurationSection GetUnityConfigurationSection()
            {
                if (!string.IsNullOrEmpty(configurationFilePath))
                {
                    if (!File.Exists(configurationFilePath))
                        throw new FileNotFoundException("Configuration file not found", configurationFilePath);
    
                    var configurationFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configurationFilePath };
                    var conf = ConfigurationManager.OpenMappedExeConfiguration(configurationFileMap, ConfigurationUserLevel.None);
    
                    return (UnityConfigurationSection)conf.Sections["unity"];
                }
    
                return (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            }
    
            #region Delegating Members
            public void Dispose()
            {
                UnderlyingContainer.Dispose();
            }
    
            public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
            {
                return UnderlyingContainer.RegisterType(from, to, name, lifetimeManager, injectionMembers);
            }
    
            public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
            {
                return UnderlyingContainer.RegisterInstance(t, name, instance, lifetime);
            }
    
            public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
            {
                return runtimeInstances.TryGetValue(t, out var result)
                    ? result.Value
                    : UnderlyingContainer.Resolve(t, name, resolverOverrides);
            }
    
            public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
            {
                return runtimeInstances.TryGetValue(t, out var result)
                    ? new[] { result.Value }
                    : UnderlyingContainer.ResolveAll(t, resolverOverrides);
            }
    
            public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
            {
                return UnderlyingContainer.BuildUp(t, existing, name, resolverOverrides);
            }
    
            public void Teardown(object o)
            {
                UnderlyingContainer.Teardown(o);
            }
    
            public IUnityContainer AddExtension(UnityContainerExtension extension)
            {
                return UnderlyingContainer.AddExtension(extension);
            }
    
            public object Configure(Type configurationInterface)
            {
                return UnderlyingContainer.Configure(configurationInterface);
            }
    
            public IUnityContainer RemoveAllExtensions()
            {
                return UnderlyingContainer.RemoveAllExtensions();
            }
    
            public IUnityContainer CreateChildContainer()
            {
                return UnderlyingContainer.CreateChildContainer();
            }
    
            public IUnityContainer Parent => UnderlyingContainer.Parent;
    
            public IEnumerable<ContainerRegistration> Registrations => UnderlyingContainer.Registrations;
            #endregion
        }
    }
}
