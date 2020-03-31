using AutomaticTypeMapper.Test.Types;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using WorkingAssembly;

namespace AutomaticTypeMapper.Test
{
    [TestFixture(typeof(UnityRegistry))]
    [TestFixture(typeof(ServiceCollectionRegistry))]
    public class RegisterDiscoveredTypesTest
    {
        private readonly Type _registryType;
        private ITypeRegistry _registry;

        public RegisterDiscoveredTypesTest(Type registryType)
        {
            Assert.That(typeof(ITypeRegistry).IsAssignableFrom(registryType));
            _registryType = registryType;
        }

        [SetUp]
        public void SetUp()
        {
            var assemblyName = Assembly.GetExecutingAssembly().FullName;
            _registry = (ITypeRegistry)Activator.CreateInstance(_registryType, assemblyName);
            _registry.RegisterDiscoveredTypes();
        }

        [TearDown]
        public void TearDown()
        {
            _registry.Dispose();
        }

        public class BasicClassTests : RegisterDiscoveredTypesTest
        {
            public BasicClassTests(Type registryType)
                : base(registryType) { }

            [Test]
            public void TaggedClass_NoParameters_IsRegistered()
            {
                var instance = _registry.Resolve<BasicClass>();
                var instance2 = _registry.Resolve<BasicClass>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void TaggedClass_WithTag_IsRegistered()
            {
                var instance = _registry.Resolve<BasicClassTagged>(nameof(BasicClassTagged));
                var instance2 = _registry.Resolve<BasicClassTagged>(nameof(BasicClassTagged));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void TaggedClass_Singleton_IsRegistered()
            {
                var instance = _registry.Resolve<BasicClassSingleton>();
                var instance2 = _registry.Resolve<BasicClassSingleton>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }

            [Test]
            public void TaggedClass_SingletonWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<BasicClassTaggedSingleton>(nameof(BasicClassTaggedSingleton));
                var instance2 = _registry.Resolve<BasicClassTaggedSingleton>(nameof(BasicClassTaggedSingleton));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }
        }

        public class InterfaceImplementationTests : RegisterDiscoveredTypesTest
        {
            public InterfaceImplementationTests(Type registryType)
                : base(registryType) { }

            [Test]
            public void TaggedClass_ImplementsInterface_IsRegistered()
            {
                var instance = _registry.Resolve<BasicInterface>();
                var instance2 = _registry.Resolve<BasicInterface>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void TaggedClass_ImplementsInterfaceWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<BasicInterfaceTagged>(nameof(InterfaceImplementationTagged));
                var instance2 = _registry.Resolve<BasicInterfaceTagged>(nameof(InterfaceImplementationTagged));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void TaggedClass_ImplementsInterfaceWithSingleton_IsRegistered()
            {
                var instance = _registry.Resolve<BasicInterfaceSingleton>();
                var instance2 = _registry.Resolve<BasicInterfaceSingleton>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }

            [Test]
            public void TaggedClass_ImplementsInterfaceWithSingletonWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<BasicInterfaceTaggedSingleton>(nameof(InterfaceImplementationTaggedSingleton));
                var instance2 = _registry.Resolve<BasicInterfaceTaggedSingleton>(nameof(InterfaceImplementationTaggedSingleton));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }
        }

        public class VariedInterfaceImplementationTests : RegisterDiscoveredTypesTest
        {
            public VariedInterfaceImplementationTests(Type registryType)
                : base(registryType) { }

            [Test]
            public void TaggedClass_SameBaseInterface_GetsRegisteredAsVariedType()
            {
                var instances = _registry.ResolveAll<VariedInterface>().ToList();
                var instances2 = _registry.ResolveAll<VariedInterface>().ToList();

                Assert.That(instances, Has.One.AssignableFrom<VariedInterfaceImplementation1>());
                Assert.That(instances, Has.One.AssignableFrom<VariedInterfaceImplementation2>());
                Assert.That(instances, Has.One.AssignableFrom<VariedInterfaceImplementation3>());
                Assert.That(instances, Has.One.AssignableFrom<VariedInterfaceImplementationSingleton1>());
                Assert.That(instances, Has.One.AssignableFrom<VariedInterfaceImplementationSingleton2>());
                Assert.That(instances, Has.One.AssignableFrom<VariedInterfaceImplementationSingleton3>());

                //not singletons
                Assert.That(instances.OfType<VariedInterfaceImplementation1>().Single(),
                            Is.Not.SameAs(instances2.OfType<VariedInterfaceImplementation1>().Single()));
                Assert.That(instances.OfType<VariedInterfaceImplementation2>().Single(),
                            Is.Not.SameAs(instances2.OfType<VariedInterfaceImplementation2>().Single()));
                Assert.That(instances.OfType<VariedInterfaceImplementation3>().Single(),
                            Is.Not.SameAs(instances2.OfType<VariedInterfaceImplementation3>().Single()));

                //singletons
                Assert.That(instances.OfType<VariedInterfaceImplementationSingleton1>().Single(),
                            Is.SameAs(instances2.OfType<VariedInterfaceImplementationSingleton1>().Single()));
                Assert.That(instances.OfType<VariedInterfaceImplementationSingleton2>().Single(),
                            Is.SameAs(instances2.OfType<VariedInterfaceImplementationSingleton2>().Single()));
                Assert.That(instances.OfType<VariedInterfaceImplementationSingleton3>().Single(),
                            Is.SameAs(instances2.OfType<VariedInterfaceImplementationSingleton3>().Single()));
            }
        }

        public class MultipleAttributesTests : RegisterDiscoveredTypesTest
        {
            public MultipleAttributesTests(Type registryType)
                : base(registryType) { }

            [Test]
            public void TaggedClass_MultipleBaseInterfaces_GetsRegisteredAsBoth()
            {
                var instance = _registry.Resolve<BaseInterface1>();
                var instance2 = _registry.Resolve<BaseInterface2>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2));
            }

            [Test]
            public void TaggedClass_MultipleBaseInterfaces_Singleton_SameInstanceRegisteredAsBoth()
            {
                var instance = _registry.Resolve<BaseInterfaceSingleton1>();
                var instance2 = _registry.Resolve<BaseInterfaceSingleton2>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2));
            }

            [Test]
            public void TaggedClass_WithSameInterfaceMultipleTimes_ThrowsInvalidOperationException()
            {
                const string assemblyName = "InvalidTypes";
                var registry = (ITypeRegistry)Activator.CreateInstance(_registryType, assemblyName);

                Assert.That(registry.RegisterDiscoveredTypes, Throws.InvalidOperationException);
            }

            [Test]
            public void TaggedClass_WithVariedInterface_AcrossAssemblies_GetsRegisteredAsVaried()
            {
                var assemblyName = Assembly.GetExecutingAssembly().FullName;
                using (var registry = (ITypeRegistry)Activator.CreateInstance(_registryType, assemblyName, "WorkingAssembly"))
                {
                    registry.RegisterDiscoveredTypes();

                    var instance = registry.Resolve<NotAcrossAssembly>();
                    var instances = registry.ResolveAll<UsedAcrossAssemblies>().ToList();

                    Assert.That(instance, Is.Not.Null);
                    Assert.That(instances, Has.Count.EqualTo(2));
                    Assert.That(instances, Has.One.TypeOf<UsedAcrossAssembliesImpl1>());
                    Assert.That(instances, Has.One.TypeOf<UsedAcrossAssembliesImpl2>());
                }
            }
        }
    }
}
