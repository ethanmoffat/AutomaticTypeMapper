using AutomaticTypeMapper.Test.Types;
using NUnit.Framework;
using System.Linq;
using System.Reflection;

namespace AutomaticTypeMapper.Test
{
    [TestFixture]
    public class RegisterAutoMappedTypesTest
    {
        public class BasicClassTests
        {
            private static ITypeRegistry _registry;

            [SetUp]
            public void Setup()
            {
                var assemblyName = Assembly.GetExecutingAssembly().FullName;
                _registry = new UnityRegistry(assemblyName);
                _registry.RegisterDiscoveredTypes();
            }

            [Test]
            public void TaggedClass_NoParameters_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicClass>();
                var instance2 = _registry.Resolve<AutoDiscovery.BasicClass>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void TaggedClass_WithTag_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicClassTagged>(nameof(AutoDiscovery.BasicClassTagged));
                var instance2 = _registry.Resolve<AutoDiscovery.BasicClassTagged>(nameof(AutoDiscovery.BasicClassTagged));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void TaggedClass_Singleton_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicClassSingleton>();
                var instance2 = _registry.Resolve<AutoDiscovery.BasicClassSingleton>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }

            [Test]
            public void TaggedClass_SingletonWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicClassTaggedSingleton>(nameof(AutoDiscovery.BasicClassTaggedSingleton));
                var instance2 = _registry.Resolve<AutoDiscovery.BasicClassTaggedSingleton>(nameof(AutoDiscovery.BasicClassTaggedSingleton));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }

            [TearDown]
            public void TearDown()
            {
                _registry.Dispose();
            }
        }

        public class InterfaceImplementationTests
        {
            private static ITypeRegistry _registry;

            [SetUp]
            public void Setup()
            {
                var assemblyName = Assembly.GetExecutingAssembly().FullName;
                _registry = new UnityRegistry(assemblyName);
                _registry.RegisterDiscoveredTypes();
            }

            [Test]
            public void TaggedClass_ImplementsInterface_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicInterface>();
                var instance2 = _registry.Resolve<AutoDiscovery.BasicInterface>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void TaggedClass_ImplementsInterfaceWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicInterfaceTagged>(nameof(InterfaceImplementationTagged));
                var instance2 = _registry.Resolve<AutoDiscovery.BasicInterfaceTagged>(nameof(InterfaceImplementationTagged));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void TaggedClass_ImplementsInterfaceWithSingleton_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicInterfaceSingleton>();
                var instance2 = _registry.Resolve<AutoDiscovery.BasicInterfaceSingleton>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }

            [Test]
            public void TaggedClass_ImplementsInterfaceWithSingletonWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicInterfaceTaggedSingleton>(nameof(InterfaceImplementationTaggedSingleton));
                var instance2 = _registry.Resolve<AutoDiscovery.BasicInterfaceTaggedSingleton>(nameof(InterfaceImplementationTaggedSingleton));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }

            [TearDown]
            public void TearDown()
            {
                _registry.Dispose();
            }
        }

        public class VariedInterfaceImplementationTests
        {
            private static ITypeRegistry _registry;

            [SetUp]
            public void Setup()
            {
                var assemblyName = Assembly.GetExecutingAssembly().FullName;
                _registry = new UnityRegistry(assemblyName);
                _registry.RegisterDiscoveredTypes();
            }

            [Test]
            public void TaggedClass_SameBaseInterface_GetsRegisteredAsVariedType()
            {
                var instances = _registry.ResolveAll<AutoDiscovery.VariedInterface>();
                var instances2 = _registry.ResolveAll<AutoDiscovery.VariedInterface>();

                Assert.That(instances, Has.One.AssignableFrom<AutoDiscovery.VariedInterfaceImplementation1>());
                Assert.That(instances, Has.One.AssignableFrom<AutoDiscovery.VariedInterfaceImplementation2>());
                Assert.That(instances, Has.One.AssignableFrom<AutoDiscovery.VariedInterfaceImplementation3>());
                Assert.That(instances, Has.One.AssignableFrom<AutoDiscovery.VariedInterfaceImplementationSingleton1>());
                Assert.That(instances, Has.One.AssignableFrom<AutoDiscovery.VariedInterfaceImplementationSingleton2>());
                Assert.That(instances, Has.One.AssignableFrom<AutoDiscovery.VariedInterfaceImplementationSingleton3>());

                //not singletons
                Assert.That(instances.OfType<AutoDiscovery.VariedInterfaceImplementation1>().Single(),
                            Is.Not.SameAs(instances2.OfType<AutoDiscovery.VariedInterfaceImplementation1>().Single()));
                Assert.That(instances.OfType<AutoDiscovery.VariedInterfaceImplementation2>().Single(),
                            Is.Not.SameAs(instances2.OfType<AutoDiscovery.VariedInterfaceImplementation2>().Single()));
                Assert.That(instances.OfType<AutoDiscovery.VariedInterfaceImplementation3>().Single(),
                            Is.Not.SameAs(instances2.OfType<AutoDiscovery.VariedInterfaceImplementation3>().Single()));

                //singletons
                Assert.That(instances.OfType<AutoDiscovery.VariedInterfaceImplementationSingleton1>().Single(),
                            Is.SameAs(instances2.OfType<AutoDiscovery.VariedInterfaceImplementationSingleton1>().Single()));
                Assert.That(instances.OfType<AutoDiscovery.VariedInterfaceImplementationSingleton2>().Single(),
                            Is.SameAs(instances2.OfType<AutoDiscovery.VariedInterfaceImplementationSingleton2>().Single()));
                Assert.That(instances.OfType<AutoDiscovery.VariedInterfaceImplementationSingleton3>().Single(),
                            Is.SameAs(instances2.OfType<AutoDiscovery.VariedInterfaceImplementationSingleton3>().Single()));
            }

            [TearDown]
            public void TearDown()
            {
                _registry.Dispose();
            }
        }

        public class MultipleAttributesTests
        {
            private static ITypeRegistry _registry;

            [SetUp]
            public void Setup()
            {
                var assemblyName = Assembly.GetExecutingAssembly().FullName;
                _registry = new UnityRegistry(assemblyName);
                _registry.RegisterDiscoveredTypes();
            }

            [Test]
            public void TaggedClass_MultipleBaseInterfaces_GetsRegisteredAsBoth()
            {
                var instance = _registry.Resolve<AutoDiscovery.BaseInterface1>();
                var instance2 = _registry.Resolve<AutoDiscovery.BaseInterface2>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2));
            }

            [Test]
            public void TaggedClass_MultipleBaseInterfaces_Singleton_SameInstanceRegisteredAsBoth()
            {
                var instance = _registry.Resolve<AutoDiscovery.BaseInterfaceSingleton1>();
                var instance2 = _registry.Resolve<AutoDiscovery.BaseInterfaceSingleton2>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2));
            }

            [Test]
            [Ignore("TODO: Invalid case for Auto base type discovery")]
            public void TaggedClass_WithSameInterfaceMultipleTimes_ThrowsInvalidOperationException()
            {
                const string assemblyName = "InvalidTypes";
                var registry = new UnityRegistry(assemblyName);

                Assert.That(registry.RegisterDiscoveredTypes, Throws.InvalidOperationException);
            }

            [TearDown]
            public void TearDown()
            {
                _registry.Dispose();
            }
        }
    }
}
