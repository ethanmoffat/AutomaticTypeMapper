using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AutomaticTypeMapper
{
    /// <summary>
    /// A type registry that uses Microsoft.Extensions.DependencyInjection.IServiceCollection
    /// </summary>
    public class ServiceCollectionRegistry : BaseRegistry
    {
        private readonly IServiceCollection _serviceCollection;

        private Lazy<IServiceProvider> ServiceProvider { get; }

        /// <summary>
        /// Create a new instance of a ServiceCollectionRegistry
        /// </summary>
        /// <param name="assemblyNames">Names of the assemblies to use for automatic type discovery</param>
        public ServiceCollectionRegistry(params string[] assemblyNames)
            : this(new ServiceCollection(), assemblyNames)
        {
            ServiceProvider = new Lazy<IServiceProvider>(() => _serviceCollection.BuildServiceProvider());
        }

        /// <summary>
        /// Create a new instance of a ServiceCollectionRegistry with the specified service collection
        /// </summary>
        /// <param name="serviceCollection">The service collection to use</param>
        /// <param name="assemblyNames">Names of the assemblies to use for automatic type discovery</param>
        public ServiceCollectionRegistry(IServiceCollection serviceCollection, params string[] assemblyNames)
            : base(assemblyNames)
        {
            _serviceCollection = serviceCollection;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterSingleton(Type type, string tag = "")
        {
            _serviceCollection.AddSingleton(type);
            return this;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterSingleton(Type type, Type baseType, string tag = "")
        {
            var serviceDescriptor = _serviceCollection.FirstOrDefault(x => x.ImplementationType == type);
            if (serviceDescriptor != null)
            {
                _serviceCollection.AddSingleton(baseType, sp => sp.GetService(serviceDescriptor.ServiceType));
            }
            else
            {
                _serviceCollection.AddSingleton(baseType, type);
            }

            return this;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterType(Type type, string tag = "")
        {
            _serviceCollection.AddTransient(type);
            return this;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterType(Type type, Type baseType, string tag = "")
        {
            _serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient(baseType, type));
            return this;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterVariedSingleton(Type type, Type baseType)
        {
            _serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton(baseType, type));
            return this;
        }

        /// <inheritdoc />
        public override ITypeRegistry RegisterVariedType(Type type, Type baseType)
        {
            RegisterType(type, baseType);
            return this;
        }

        /// <inheritdoc />
        public override T Resolve<T>(string tag = "")
        {
            return ServiceProvider.Value.GetRequiredService<T>();
        }

        /// <inheritdoc />
        public override IEnumerable<T> ResolveAll<T>()
        {
            return ServiceProvider.Value.GetServices<T>();
        }
    }
}
