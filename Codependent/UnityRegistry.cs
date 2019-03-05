using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Resolution;

namespace Codependent
{
    /// <summary>
    /// A type registry that uses UnityContainer as the backing container
    /// </summary>
    public class UnityRegistry : ITypeRegistry
    {
        private Assembly[] _assemblies;

        /// <summary>
        /// Unity container backing this instance of UnityRegistry
        /// </summary>
        public IUnityContainer UnityContainer { get;}

        /// <summary>
        /// Create a new registry using the specified assembly names for type discovery
        /// </summary>
        /// <param name="assemblyNames">Names of the assemblies to use for automatic type discovery</param>
        public UnityRegistry(params string[] assemblyNames)
            : this(new UnityContainer(), assemblyNames) { }

        /// <summary>
        /// Create a new registry using the specified unity container and assembly names for type discovery
        /// </summary>
        /// <param name="container">Unity container to use for type registration</param>
        /// <param name="assemblyNames">Names of the assemblies to use for automatic type discovery</param>
        public UnityRegistry(UnityContainer container, params string[] assemblyNames)
        {
            _assemblies = assemblyNames.Select(Assembly.Load).ToArray();
            UnityContainer = container;
        }

        private UnityRegistry(UnityContainer container, Assembly[] assemblies)
        {
            _assemblies = assemblies;
            UnityContainer = container;
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterDiscoveredTypes()
        {
            foreach (Assembly assembly in _assemblies)
            {
                var typeAttributeSets = assembly.GetTypes()
                    .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(CodependentTypeAttribute)))
                    .Select(x => new
                                 {
                                    Type = x,
                                    Implements = x.GetCustomAttributes(typeof(CodependentTypeAttribute))
                                                  .Cast<CodependentTypeAttribute>()
                                                  .ToList()
                                 });

                var baseTypeCount = typeAttributeSets.SelectMany(x => x.Implements)
                                                     .Where(x => x.BaseType != null)
                                                     .GroupBy(x => x.BaseType)
                                                     .ToDictionary(k => k.First().BaseType, v => v.Count());

                foreach (var typeAttributeSet in typeAttributeSets)
                {
                    foreach (var implementsAttribute in typeAttributeSet.Implements)
                    {
                        if (implementsAttribute.IsSingleton)
                        {
                            if (implementsAttribute.BaseType == null)
                            {
                                RegisterSingleton(typeAttributeSet.Type, implementsAttribute.Tag);
                            }
                            else
                            {
                                if (baseTypeCount[implementsAttribute.BaseType] > 1)
                                    RegisterVariedSingleton(typeAttributeSet.Type, implementsAttribute.BaseType);
                                else
                                    RegisterSingleton(typeAttributeSet.Type, implementsAttribute.BaseType, implementsAttribute.Tag);
                            }
                        }
                        else
                        {
                            if (implementsAttribute.BaseType == null)
                            {
                                RegisterType(typeAttributeSet.Type, implementsAttribute.Tag);
                            }
                            else
                            {
                                if (baseTypeCount[implementsAttribute.BaseType] > 1)
                                    RegisterVariedType(typeAttributeSet.Type, implementsAttribute.BaseType);
                                else
                                    RegisterType(typeAttributeSet.Type, implementsAttribute.BaseType, implementsAttribute.Tag);
                            }
                        }
                    }
                }
            }

            return this;
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterType(Type type, string tag = "")
        {
            if (string.IsNullOrWhiteSpace(tag))
                UnityContainer.RegisterType(type);
            else
                UnityContainer.RegisterType(type, tag);

            return this;
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterType(Type type, Type baseType, string tag = "")
        {
            if (string.IsNullOrWhiteSpace(tag))
                UnityContainer.RegisterType(baseType, type);
            else
                UnityContainer.RegisterType(baseType, type, tag);

            return this;
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterVariedType(Type type, Type baseType)
        {
            RegisterEnumerableIfNeeded(UnityContainer, baseType);
            UnityContainer.RegisterType(baseType, type, type.Name);
            return this;
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterSingleton(Type type, string tag = "")
        {
            if (string.IsNullOrWhiteSpace(tag))
                UnityContainer.RegisterType(type, new ContainerControlledLifetimeManager());
            else
                UnityContainer.RegisterType(type, tag, new ContainerControlledLifetimeManager());

            return this;
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterSingleton(Type type, Type baseType, string tag = "")
        {
            if (string.IsNullOrWhiteSpace(tag))
                UnityContainer.RegisterType(baseType, type, new ContainerControlledLifetimeManager());
            else
                UnityContainer.RegisterType(baseType, type, tag, new ContainerControlledLifetimeManager());

            return this;
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterVariedSingleton(Type type, Type baseType)
        {
            RegisterEnumerableIfNeeded(UnityContainer, baseType);
            UnityContainer.RegisterType(baseType, type, type.Name, new ContainerControlledLifetimeManager());
            return this;
        }

        /// <inheritdoc />
        public T Resolve<T>(string tag = "")
        {
            return string.IsNullOrWhiteSpace(tag)
                ? UnityContainer.Resolve<T>()
                : UnityContainer.Resolve<T>(tag);
        }

        /// <inheritdoc />
        public IEnumerable<T> ResolveAll<T>()
        {
            return UnityContainer.ResolveAll<T>();
        }

        private static void RegisterEnumerableIfNeeded(IUnityContainer container, Type baseType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(new Type[] { baseType });
            if (!container.IsRegistered(enumerableType))
            {
                container.RegisterFactory(
                    enumerableType,
                    c => c.ResolveAll(baseType),
                    new ContainerControlledLifetimeManager());
            }
        }
    }
}
