using System;
using System.Collections.Generic;
using Unity;

namespace Codependent
{
    public interface ITypeRegistry
    {
        /// <summary>
        /// Registers all types that have the 'Implements' attribute applied to them
        /// </summary>
        /// <returns>Type registry (for method chaining)</returns>
        ITypeRegistry RegisterDiscoveredTypes();

        ITypeRegistry RegisterType(Type type, string tag = "");

        ITypeRegistry RegisterType(Type type, Type baseType, string tag = "");

        ITypeRegistry RegisterVariedType(Type type, Type baseType, string tag = "");

        ITypeRegistry RegisterSingleton(Type type, string tag = "");

        ITypeRegistry RegisterSingleton(Type type, Type baseType, string tag = "");

        ITypeRegistry RegisterVariedSingleton(Type type, Type baseType, string tag = "");

        T Resolve<T>(string tag = "");

        IEnumerable<T> ResolveAll<T>(string tag = "");
    }
}