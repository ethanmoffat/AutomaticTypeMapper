using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
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
                var mappedTypeAttributeSets = GetTypeAttributeSets<MappedTypeAttribute>(assembly);
                var autoSets = GetTypeAttributeSets<AutoMappedTypeAttribute>(assembly);

                var typesWithBothAttributes = mappedTypeAttributeSets.Select(x => x.Type).Intersect(autoSets.Select(x => x.Type)).ToList();
                if (typesWithBothAttributes.Any())
                {
                    var typeName = typesWithBothAttributes.First();
                    throw new InvalidOperationException($"MappedType and AutoMappedType are not supported on the same type ({typeName}).");
                }

                mappedTypeAttributeSets.AddRange(GetMappedTypeAttributesFromAutoAttributes(autoSets));

                var multipleSameBaseTypesForImplementingType = mappedTypeAttributeSets.Where(
                    set => set.MappedTypes.GroupBy(x => x.BaseType).Any(x => x.Count() > 1)).ToList();
                if (multipleSameBaseTypesForImplementingType.Any())
                {
                    var typeName = multipleSameBaseTypesForImplementingType.First().Type.Name;
                    throw new InvalidOperationException($"Type {typeName} is mapped to the same base type multiple times");
                }

                var baseTypeCount = mappedTypeAttributeSets.SelectMany(x => x.MappedTypes)
                                                           .Where(x => x.BaseType != null)
                                                           .GroupBy(x => x.BaseType)
                                                           .ToDictionary(k => k.First().BaseType, v => v.Count());

                RegisterDiscoveredTypes(mappedTypeAttributeSets, baseTypeCount);
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
        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
        }

        private static List<TypeAttributeSet<T>> GetTypeAttributeSets<T>(Assembly assembly)
            where T : Attribute
        {
            return assembly.GetTypes()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(T)))
                .Select(x => new TypeAttributeSet<T>(
                    type: x,
                    mappedTypes: x.GetCustomAttributes(typeof(T))
                                  .Cast<T>()
                                  .ToList()))
                .ToList();
        }

        private static IEnumerable<TypeAttributeSet<MappedTypeAttribute>> GetMappedTypeAttributesFromAutoAttributes(IEnumerable<TypeAttributeSet<AutoMappedTypeAttribute>> autoSets)
        {
            foreach (var autoSet in autoSets)
            {
                var attribute = autoSet.MappedTypes.Single();
                var baseTypes = autoSet.Type.FindInterfaces((t, o) => t.IsInterface, null);

                var mappedTypeAttributeList = baseTypes.Select(bt => new MappedTypeAttribute(bt, attribute.IsSingleton, attribute.Tag)).ToList();
                if (!mappedTypeAttributeList.Any())
                    mappedTypeAttributeList.Add(new MappedTypeAttribute(null, attribute.IsSingleton, attribute.Tag));

                yield return new TypeAttributeSet<MappedTypeAttribute>(
                    autoSet.Type,
                    mappedTypeAttributeList
                );
            }
        }

        private void RegisterDiscoveredTypes(IEnumerable<TypeAttributeSet<MappedTypeAttribute>> mappedTypeAttributeSets,
                                             IDictionary<Type, int> baseTypeCount)
        {
            foreach (var typeAttributeSet in mappedTypeAttributeSets)
            {
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
    }
}
