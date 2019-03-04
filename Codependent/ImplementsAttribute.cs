using System;

namespace Codependent
{
    /// <summary>
    /// Attribute for automatic type registrating in Codependent
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ImplementsAttribute : Attribute
    {
        /// <summary>
        /// The base type that the tagged class implements (optional)
        /// </summary>
        public Type BaseType { get; }

        /// <summary>
        /// True if the type is a singleton (e.g. state must be preserved in the registry)
        /// </summary>
        public bool IsSingleton { get; }

        /// <summary>
        /// Tag for the type (optional)
        /// </summary>
        public string Tag { get; }

        public ImplementsAttribute()
            : this(null) { }

        public ImplementsAttribute(Type baseType)
            : this(baseType, false) { }

        public ImplementsAttribute(Type baseType, bool singleton)
            : this(baseType, singleton, string.Empty) { }

        public ImplementsAttribute(Type baseType, bool singleton, string tag)
        {
            BaseType = baseType;
            IsSingleton = singleton;
            Tag = tag;
        }
    }
}
