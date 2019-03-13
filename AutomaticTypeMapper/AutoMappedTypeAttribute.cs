using System;

namespace AutomaticTypeMapper
{
    /// <summary>
    /// Attribute for automatic type registration without specification of explicit base type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AutoMappedTypeAttribute : Attribute
    {
        /// <summary>
        /// True if the type is a singleton (e.g. state must be preserved in the registry)
        /// </summary>
        public bool IsSingleton { get; set; }

        /// <summary>
        /// Tag for the type (optional). Tag will be ignored for varied types
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Create a default instance of AutoMappedTypeAttribute
        /// </summary>
        public AutoMappedTypeAttribute()
            : this(false) { }

        /// <summary>
        /// Create an instance of AutoMappedTypeAttribute with specified singleton state
        /// </summary>
        /// <param name="isSingleton">True if the mapped type should be a singleton</param>
        public AutoMappedTypeAttribute(bool isSingleton)
            : this(isSingleton, string.Empty) { }

        /// <summary>
        /// Create an instance of AutoMappedTypeAttribute with specified singleton state and tag
        /// </summary>
        /// <param name="isSingleton">True if the mapped type should be a singleton</param>
        /// <param name="tag">The tag to register with the type</param>
        public AutoMappedTypeAttribute(bool isSingleton, string tag)
        {
            IsSingleton = isSingleton;
            Tag = tag;
        }
    }
}
