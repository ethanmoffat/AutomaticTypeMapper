namespace Codependent
{
    /// <summary>
    /// Extensions to make registration of types in an ITypeRegistry easier
    /// </summary>
    public static class TypeRegistryExtensions
    {
        /// <summary>
        /// Registers specified type
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="registry">The registry in which the type should be registered</param>
        /// <param name="tag">Tag for the type (optional)</param>
        /// <returns>Type registry (for method chaining)</returns>
        public static ITypeRegistry RegisterType<T>(this ITypeRegistry registry, string tag = "")
        {
            registry.RegisterType(typeof(T), tag);
            return registry;
        }

        /// <summary>
        /// Registers specified type implementing a base type
        /// </summary>
        /// <typeparam name="TBase">Base type that the type implements</typeparam>
        /// <typeparam name="TType">Type to register</typeparam>
        /// <param name="registry">The registry in which the type should be registered</param>
        /// <param name="tag">Tag for the type (optional)</param>
        /// <returns>Type registry (for method chaining)</returns>
        public static ITypeRegistry RegisterType<TBase, TType>(this ITypeRegistry registry, string tag = "") where TType : TBase
        {
            registry.RegisterType(typeof(TType), typeof(TBase), tag);
            return registry;
        }

        /// <summary>
        /// Registers specified varied type implementing a base type. A varied type is one of many implementing classes for an interface
        /// </summary>
        /// <typeparam name="TBase">Base type that the type implements</typeparam>
        /// <typeparam name="TType">Type to register</typeparam>
        /// <param name="registry">The registry in which the type should be registered</param>
        /// <returns>Type registry (for method chaining)</returns>
        public static ITypeRegistry RegisterVariedType<TBase, TType>(this ITypeRegistry registry) where TType : TBase
        {
            registry.RegisterVariedType(typeof(TType), typeof(TBase));
            return registry;
        }

        /// <summary>
        /// Registers specified type as a singleton
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="registry">The registry in which the type should be registered</param>
        /// <param name="tag">Tag for the type (optional)</param>
        /// <returns>Type registry (for method chaining)</returns>
        public static ITypeRegistry RegisterSingleton<T>(this ITypeRegistry registry, string tag = "")
        {
            registry.RegisterSingleton(typeof(T), tag);
            return registry;
        }

        /// <summary>
        /// Registers specified type implementing a base type as a singleton
        /// </summary>
        /// <typeparam name="TBase">Base type that the type implements</typeparam>
        /// <typeparam name="TType">Type to register</typeparam>
        /// <param name="registry">The registry in which the type should be registered</param>
        /// <param name="tag">Tag for the type (optional)</param>
        /// <returns>Type registry (for method chaining)</returns>
        public static ITypeRegistry RegisterSingleton<TBase, TType>(this ITypeRegistry registry, string tag = "") where TType : TBase
        {
            registry.RegisterSingleton(typeof(TType), typeof(TBase), tag);
            return registry;
        }

        /// <summary>
        /// Registers specified varied type implementing a base type as a singleton. A varied type is one of many implementing classes for an interface
        /// </summary>
        /// <typeparam name="TBase">Base type that the type implements</typeparam>
        /// <typeparam name="TType">Type to register</typeparam>
        /// <param name="registry">The registry in which the type should be registered</param>
        /// <returns>Type registry (for method chaining)</returns>
        public static ITypeRegistry RegisterVariedSingleton<TBase, TType>(this ITypeRegistry registry) where TType : TBase
        {
            registry.RegisterVariedSingleton(typeof(TType), typeof(TBase));
            return registry;
        }
    }
}
