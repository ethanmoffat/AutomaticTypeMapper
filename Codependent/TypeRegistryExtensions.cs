namespace Codependent
{
    public static class TypeRegistryExtensions
    {
        public static ITypeRegistry RegisterType<T>(this ITypeRegistry registry, string tag = "")
        {
            registry.RegisterType(typeof(T), tag);
            return registry;
        }

        public static ITypeRegistry RegisterType<TBase, TType>(this ITypeRegistry registry, string tag = "") where TType : TBase
        {
            registry.RegisterType(typeof(TType), typeof(TBase), tag);
            return registry;
        }

        public static ITypeRegistry RegisterVariedType<TBase, TType>(this ITypeRegistry registry, string tag = "") where TType : TBase
        {
            registry.RegisterVariedType(typeof(TType), typeof(TBase), tag);
            return registry;
        }

        public static ITypeRegistry RegisterSingleton<T>(this ITypeRegistry registry, string tag = "")
        {
            registry.RegisterSingleton(typeof(T), tag);
            return registry;
        }

        public static ITypeRegistry RegisterSingleton<TBase, TType>(this ITypeRegistry registry, string tag = "") where TType : TBase
        {
            registry.RegisterSingleton(typeof(TType), typeof(TBase), tag);
            return registry;
        }

        public static ITypeRegistry RegisterVariedSingleton<TBase, TType>(this ITypeRegistry registry, string tag = "") where TType : TBase
        {
            registry.RegisterVariedSingleton(typeof(TType), typeof(TBase), tag);
            return registry;
        }
    }
}
