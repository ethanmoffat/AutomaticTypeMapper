using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity;

namespace Codependent
{
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
                    .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ImplementsAttribute)))
                    .Select(x => new
                                 {
                                    Type = x,
                                    Implements = x.GetCustomAttributes(typeof(ImplementsAttribute))
                                                  .Cast<ImplementsAttribute>()
                                                  .ToList()
                                 });

                foreach (var typeAttributeSet in typeAttributeSets)
                {
                    foreach (var implementsAttribute in typeAttributeSet.Implements)
                    {
                        if (implementsAttribute.IsSingleton)
                        {
                            if (implementsAttribute.BaseType == null)
                                RegisterSingleton(typeAttributeSet.Type, implementsAttribute.Tag);
                            else
                                RegisterSingleton(typeAttributeSet.Type, implementsAttribute.BaseType, implementsAttribute.Tag);
                        }
                        else
                        {
                            if (implementsAttribute.BaseType == null)
                                RegisterType(typeAttributeSet.Type, implementsAttribute.Tag);
                            else
                                RegisterType(typeAttributeSet.Type, implementsAttribute.BaseType, implementsAttribute.Tag);
                        }
                    }
                }
            }

            return this;
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterType(Type type, string tag = "")
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterType(Type type, Type baseType, string tag = "")
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterVariedType(Type type, Type baseType, string tag = "")
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterSingleton(Type type, string tag = "")
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterSingleton(Type type, Type baseType, string tag = "")
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeRegistry RegisterVariedSingleton(Type type, Type baseType, string tag = "")
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public T Resolve<T>(string tag = "")
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<T> ResolveAll<T>(string tag = "")
        {
            throw new NotImplementedException();
        }
    }
}
