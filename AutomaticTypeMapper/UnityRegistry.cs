using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity;
using Unity.Lifetime;

namespace AutomaticTypeMapper
{
    /// <summary>
    /// A type registry that uses UnityContainer as the backing container
    /// </summary>
    public class UnityRegistry : BaseRegistry
    {
        private readonly bool _containerNeedsDispose;

        /// <summary>
        /// Unity container backing this instance of UnityRegistry
        /// </summary>
        public IUnityContainer UnityContainer { get; }

        /// <inheritdoc />
        public UnityRegistry(params string[] assemblyNames)
            : this(new UnityContainer(), assemblyNames)
        {
            _containerNeedsDispose = true;
        }

        /// <summary>
        /// Create a new registry using the specified unity container and assembly names for type discovery
        /// </summary>
        /// <param name="container">Unity container to use for type registration</param>
        /// <param name="assemblyNames">Names of the assemblies to use for automatic type discovery</param>
        public UnityRegistry(IUnityContainer container, params string[] assemblyNames)
            : base(assemblyNames)
        {
            UnityContainer = container;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterType(Type type, string tag = "")
        {
            if (string.IsNullOrWhiteSpace(tag))
                UnityContainer.RegisterType(type);
            else
                UnityContainer.RegisterType(type, tag);

            return this;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterType(Type type, Type baseType, string tag = "")
        {
            if (string.IsNullOrWhiteSpace(tag))
                UnityContainer.RegisterType(baseType, type);
            else
                UnityContainer.RegisterType(baseType, type, tag);

            return this;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterVariedType(Type type, Type baseType)
        {
            RegisterEnumerableIfNeeded(UnityContainer, baseType);
            UnityContainer.RegisterType(baseType, type, type.Name);
            return this;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterSingleton(Type type, string tag = "")
        {
            if (string.IsNullOrWhiteSpace(tag))
                UnityContainer.RegisterType(type, new ContainerControlledLifetimeManager());
            else
                UnityContainer.RegisterType(type, tag, new ContainerControlledLifetimeManager());

            return this;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterSingleton(Type type, Type baseType, string tag = "")
        {
            RegisterInstanceIfNeeded(UnityContainer, type, tag);

            if (string.IsNullOrWhiteSpace(tag))
                UnityContainer.RegisterType(baseType, type);
            else
                UnityContainer.RegisterType(baseType, type, tag);

            return this;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterVariedSingleton(Type type, Type baseType)
        {
            RegisterEnumerableIfNeeded(UnityContainer, baseType);
            RegisterInstanceIfNeeded(UnityContainer, type, type.Name);
            UnityContainer.RegisterType(baseType, type, type.Name);
            return this;
        }

        /// <inheritdoc />
        public override T Resolve<T>(string tag = "")
        {
            return string.IsNullOrWhiteSpace(tag)
                ? UnityContainer.Resolve<T>()
                : UnityContainer.Resolve<T>(tag);
        }

        /// <inheritdoc />
        public override IEnumerable<T> ResolveAll<T>()
        {
            return UnityContainer.ResolveAll<T>();
        }

        private static void RegisterEnumerableIfNeeded(IUnityContainer container, Type baseType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(baseType);
            if (!container.IsRegistered(enumerableType))
            {
                container.RegisterFactory(
                    enumerableType,
                    c => c.ResolveAll(baseType),
                    new ContainerControlledLifetimeManager());
            }
        }

        private static void RegisterInstanceIfNeeded(IUnityContainer container, Type type, string tag = "")
        {
            if (!container.IsRegistered(type))
            {
                if (string.IsNullOrWhiteSpace(tag))
                    container.RegisterType(type, new ContainerControlledLifetimeManager());
                else
                    container.RegisterType(type, tag, new ContainerControlledLifetimeManager());
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_containerNeedsDispose)
                { 
                    UnityContainer.Dispose();
                }
            }
        }
    }
}
