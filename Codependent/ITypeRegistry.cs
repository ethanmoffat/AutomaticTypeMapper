using System;
using System.Collections.Generic;
using Unity;

namespace Codependent
{
    /// <summary>
    /// Represents a type registry. May be implemented by Unity or other IoC container systems.
    /// </summary>
    public interface ITypeRegistry
    {
        /// <summary>
        /// Registers all types that have the 'Implements' attribute applied to them
        /// </summary>
        /// <returns>Type registry (for method chaining)</returns>
        ITypeRegistry RegisterDiscoveredTypes();

        /// <summary>
        /// Registers specified type
        /// </summary>
        /// <param name="type">Type to register</param>
        /// <param name="tag">Tag for the type (optional)</param>
        /// <returns>Type registry (for method chaining)</returns>
        ITypeRegistry RegisterType(Type type, string tag = "");

        /// <summary>
        /// Registers specified type implementing a base type
        /// </summary>
        /// <param name="type">Type to register</param>
        /// <param name="baseType">Base type that the type implements</param>
        /// <param name="tag">Tag for the type (optional)</param>
        /// <returns>Type registry (for method chaining)</returns>
        ITypeRegistry RegisterType(Type type, Type baseType, string tag = "");

        /// <summary>
        /// Registers specified varied type implementing a base type. A varied type is one of many implementing classes for an interface
        /// </summary>
        /// <param name="type">Type to register</param>
        /// <param name="baseType">Base type that the type implements</param>
        /// <returns>Type registry (for method chaining)</returns>
        ITypeRegistry RegisterVariedType(Type type, Type baseType);

        /// <summary>
        /// Registers specified type as a singleton
        /// </summary>
        /// <param name="type">Type to register</param>
        /// <param name="tag">Tag for the type (optional)</param>
        /// <returns>Type registry (for method chaining)</returns>
        ITypeRegistry RegisterSingleton(Type type, string tag = "");

        /// <summary>
        /// Registers specified type implementing a base type as a singleton
        /// </summary>
        /// <param name="type">Type to register</param>
        /// <param name="baseType">Base type that the type implements</param>
        /// <param name="tag">Tag for the type (optional)</param>
        /// <returns>Type registry (for method chaining)</returns>
        ITypeRegistry RegisterSingleton(Type type, Type baseType, string tag = "");

        /// <summary>
        /// Registers specified varied type implementing a base type as a singleton. A varied type is one of many implementing classes for an interface
        /// </summary>
        /// <param name="type">Type to register</param>
        /// <param name="baseType">Base type that the type implements</param>
        /// <returns>Type registry (for method chaining)</returns>
        ITypeRegistry RegisterVariedSingleton(Type type, Type baseType);

        /// <summary>
        /// Resolve the specified type
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="tag">Tag for the type (optional)</param>
        /// <returns>Instance of the requested type</returns>
        T Resolve<T>(string tag = "");

        /// <summary>
        /// Resolve all implementing classes of the specified type T registered using RegisterVaried*
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Instances of all implementing interfaces of the registered type</returns>
        IEnumerable<T> ResolveAll<T>();
    }
}