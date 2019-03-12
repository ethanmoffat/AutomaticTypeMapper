using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutomaticTypeMapper.Internal;

namespace AutomaticTypeMapper
{
    /// <summary>
    /// Base registry class that contains logic for automatic type discovery
    /// </summary>
    public abstract class BaseRegistry : ITypeRegistry
    {
        private readonly Assembly[] _assemblies;

        /// <summary>
        /// Create a new registry using the specified assembly names for type discovery
        /// </summary>
        /// <param name="assemblyNames">Names of the assemblies to use for automatic type discovery</param>
        protected BaseRegistry(params string[] assemblyNames)
        {
            _assemblies = assemblyNames.Select(Assembly.Load).ToArray();
        }

        /// <inheritdoc />
        public virtual ITypeRegistry RegisterDiscoveredTypes()
        {
            foreach (var assembly in _assemblies)
            {
                var typeAttributeSets = assembly.GetTypes()
                    .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(MappedTypeAttribute)))
                    .Select(x => new TypeAttributeSet<MappedTypeAttribute>(
                        type: x,
                        mappedTypes: x.GetCustomAttributes(typeof(MappedTypeAttribute))
                                      .Cast<MappedTypeAttribute>()
                                      .ToList()))
                    .ToList();

                var baseTypeCount = typeAttributeSets.SelectMany(x => x.MappedTypes)
                                                     .Where(x => x.BaseType != null)
                                                     .GroupBy(x => x.BaseType)
                                                     .ToDictionary(k => k.First().BaseType, v => v.Count());

                foreach (var typeAttributeSet in typeAttributeSets)
                {
                    var invalidCases = typeAttributeSet.MappedTypes
                                                       .GroupBy(x => x.BaseType)
                                                       .Where(x => x.Count() > 1)
                                                       .ToList();

                    if (invalidCases.Any())
                    {
                        var sb = BuildTypeDiscoveryErrorMessage(typeAttributeSet.Type.Name, invalidCases);
                        throw new InvalidOperationException(sb.ToString());
                    }

                    foreach (var mapping in typeAttributeSet.MappedTypes)
                    {
                        if (mapping.IsSingleton)
                        {
                            if (mapping.BaseType == null)
                            {
                                RegisterSingleton(typeAttributeSet.Type, mapping.Tag);
                            }
                            else
                            {
                                if (baseTypeCount[mapping.BaseType] > 1)
                                    RegisterVariedSingleton(typeAttributeSet.Type, mapping.BaseType);
                                else
                                    RegisterSingleton(typeAttributeSet.Type, mapping.BaseType, mapping.Tag);
                            }
                        }
                        else
                        {
                            if (mapping.BaseType == null)
                            {
                                RegisterType(typeAttributeSet.Type, mapping.Tag);
                            }
                            else
                            {
                                if (baseTypeCount[mapping.BaseType] > 1)
                                    RegisterVariedType(typeAttributeSet.Type, mapping.BaseType);
                                else
                                    RegisterType(typeAttributeSet.Type, mapping.BaseType, mapping.Tag);
                            }
                        }
                    }
                }
            }

            return this;
        }

        /// <inheritdoc />
        public abstract ITypeRegistry RegisterType(Type type, string tag = "");

        /// <inheritdoc />
        public abstract ITypeRegistry RegisterType(Type type, Type baseType, string tag = "");

        /// <inheritdoc />
        public abstract ITypeRegistry RegisterVariedType(Type type, Type baseType);

        /// <inheritdoc />
        public abstract ITypeRegistry RegisterSingleton(Type type, string tag = "");

        /// <inheritdoc />
        public abstract ITypeRegistry RegisterSingleton(Type type, Type baseType, string tag = "");

        /// <inheritdoc />
        public abstract ITypeRegistry RegisterVariedSingleton(Type type, Type baseType);

        /// <inheritdoc />
        public abstract T Resolve<T>(string tag = "");

        /// <inheritdoc />
        public abstract IEnumerable<T> ResolveAll<T>();

        /// <inheritdoc />
        ~BaseRegistry()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose managed and unmanaged resources created by this instance
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if called from finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        private static StringBuilder BuildTypeDiscoveryErrorMessage(string typeName, IEnumerable<IGrouping<Type, MappedTypeAttribute>> invalidCases)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Type {typeName} is mapped to the following types multiple times:");

            foreach (var invalid in invalidCases.SelectMany(x => x.Select(y => y.BaseType.Name)).Distinct())
                sb.AppendLine($"  - {invalid}");

            return sb;
        }
    }
}
