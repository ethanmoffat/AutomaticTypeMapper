using System;

namespace AutomaticTypeMapper
{
    /// <summary>
    /// Attribute for automatic type registrating in Codependent
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CodependentTypeAttribute : Attribute
    {
        /// <summary>
        /// The base type that the tagged class implements (optional)
        /// </summary>
        public Type BaseType { get; set; }

        /// <summary>
        /// True if the type is a singleton (e.g. state must be preserved in the registry)
        /// </summary>
        public bool IsSingleton { get; set; }

        /// <summary>
        /// Tag for the type (optional). Tag will be ignored for varied types
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Discover the tagged type automatically when calling ITypeRegistry.RegisterDiscoveredTypes
        /// </summary>
        public CodependentTypeAttribute()
            : this(null) { }

        /// <summary>
        /// Discover the tagged type automatically when calling ITypeRegistry.RegisterDiscoveredTypes.
        /// </summary>
        /// <param name="baseType">The type that the tagged type implements</param>
        public CodependentTypeAttribute(Type baseType)
            : this(baseType, false) { }

        /// <summary>
        /// Discover the tagged type automatically when calling ITypeRegistry.RegisterDiscoveredTypes.
        /// </summary>
        /// <param name="baseType">The type that the tagged type implements</param>
        /// <param name="singleton">True if the type should be registered as a singleton (container uses one instance of the object)</param>
        public CodependentTypeAttribute(Type baseType, bool singleton)
            : this(baseType, singleton, string.Empty) { }

        /// <summary>
        /// Discover the tagged type automatically when calling ITypeRegistry.RegisterDiscoveredTypes.
        /// </summary>
        /// <param name="baseType">The type that the tagged type implements</param>
        /// <param name="singleton">True if the type should be registered as a singleton (container uses one instance of the object)</param>
        /// <param name="tag">Tag for the type (optional). Tag will be ignored for varied types</param>
        public CodependentTypeAttribute(Type baseType, bool singleton, string tag)
        {
            BaseType = baseType;
            IsSingleton = singleton;
            Tag = tag;
        }
    }
}
