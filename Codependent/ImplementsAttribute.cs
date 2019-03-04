using System;

namespace Codependent
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ImplementsAttribute : Attribute
    {
        internal const string BaseTypeName = nameof(BaseType);
        internal const string IsSingletonName = nameof(IsSingleton);
        internal const string TagName = nameof(Tag);

        public Type BaseType { get; }

        public bool IsSingleton { get; }

        public string Tag { get; }

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
