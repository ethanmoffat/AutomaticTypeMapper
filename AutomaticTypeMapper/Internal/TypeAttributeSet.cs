using System;
using System.Collections.Generic;

namespace AutomaticTypeMapper.Internal
{
    internal class TypeAttributeSet<T> where T : Attribute
    {
        internal Type Type { get; }

        internal IList<T> MappedTypes { get; }

        internal TypeAttributeSet(Type type, IList<T> mappedTypes)
        {
            Type = type;
            MappedTypes = mappedTypes;
        }
    }
}
