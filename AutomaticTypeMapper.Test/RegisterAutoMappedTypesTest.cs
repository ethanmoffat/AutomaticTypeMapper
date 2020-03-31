using AutomaticTypeMapper.Test.Types;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace AutomaticTypeMapper.Test
{
    [TestFixture(typeof(UnityRegistry))]
    [TestFixture(typeof(ServiceCollectionRegistry))]
    public class RegisterAutoMappedTypesTest
    {
        private readonly Type _registryType;
        private ITypeRegistry _registry;

        public RegisterAutoMappedTypesTest(Type registryType)
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

        public class BasicClassTests : RegisterAutoMappedTypesTest
        {
            public BasicClassTests(Type registryType)
                : base(registryType) { }

            [Test]
            public void AutoMappedClass_NoParameters_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicClass>();
                var instance2 = _registry.Resolve<AutoDiscovery.BasicClass>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void AutoMappedClass_WithTag_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicClassTagged>(nameof(AutoDiscovery.BasicClassTagged));
                var instance2 = _registry.Resolve<AutoDiscovery.BasicClassTagged>(nameof(AutoDiscovery.BasicClassTagged));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void AutoMappedClass_Singleton_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicClassSingleton>();
                var instance2 = _registry.Resolve<AutoDiscovery.BasicClassSingleton>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }

            [Test]
            public void AutoMappedClass_SingletonWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicClassTaggedSingleton>(nameof(AutoDiscovery.BasicClassTaggedSingleton));
                var instance2 = _registry.Resolve<AutoDiscovery.BasicClassTaggedSingleton>(nameof(AutoDiscovery.BasicClassTaggedSingleton));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }
        }

        public class InterfaceImplementationTests : RegisterAutoMappedTypesTest
        {
            public InterfaceImplementationTests(Type registryType)
                : base(registryType) { }

            [Test]
            public void AutoMappedClass_ImplementsInterface_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicInterface>();
                var instance2 = _registry.Resolve<AutoDiscovery.BasicInterface>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void AutoMappedClass_ImplementsInterfaceWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicInterfaceTagged>(nameof(InterfaceImplementationTagged));
                var instance2 = _registry.Resolve<AutoDiscovery.BasicInterfaceTagged>(nameof(InterfaceImplementationTagged));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2)); //not singleton
            }

            [Test]
            public void AutoMappedClass_ImplementsInterfaceWithSingleton_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicInterfaceSingleton>();
                var instance2 = _registry.Resolve<AutoDiscovery.BasicInterfaceSingleton>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }

            [Test]
            public void AutoMappedClass_ImplementsInterfaceWithSingletonWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<AutoDiscovery.BasicInterfaceTaggedSingleton>(nameof(InterfaceImplementationTaggedSingleton));
                var instance2 = _registry.Resolve<AutoDiscovery.BasicInterfaceTaggedSingleton>(nameof(InterfaceImplementationTaggedSingleton));

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2)); //singleton
            }
        }

        public class VariedInterfaceImplementationTests : RegisterAutoMappedTypesTest
        {
            public VariedInterfaceImplementationTests(Type registryType)
                : base(registryType) { }

            [Test]
            public void AutoMappedClass_SameBaseInterface_GetsRegisteredAsVariedType()
            {
                var instances = _registry.ResolveAll<AutoDiscovery.VariedInterface>().ToList();
                var instances2 = _registry.ResolveAll<AutoDiscovery.VariedInterface>().ToList();

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
        }

        public class MultipleAttributesTests : RegisterAutoMappedTypesTest
        {
            public MultipleAttributesTests(Type registryType)
                : base(registryType) { }

            [Test]
            public void AutoMappedClass_MultipleBaseInterfaces_GetsRegisteredAsBoth()
            {
                var instance = _registry.Resolve<AutoDiscovery.BaseInterface1>();
                var instance2 = _registry.Resolve<AutoDiscovery.BaseInterface2>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2));
            }

            [Test]
            public void AutoMappedClass_MultipleBaseInterfaces_Singleton_SameInstanceRegisteredAsBoth()
            {
                var instance = _registry.Resolve<AutoDiscovery.BaseInterfaceSingleton1>();
                var instance2 = _registry.Resolve<AutoDiscovery.BaseInterfaceSingleton2>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.SameAs(instance2));
            }

            [Test]
            public void AutoMappedClass_WithSameInterfaceMultipleTimes_ThrowsInvalidOperationException()
            {
                const string assemblyName = "InvalidAutoTypes";
                var registry = (ITypeRegistry)Activator.CreateInstance(_registryType, assemblyName);

                Assert.That(registry.RegisterDiscoveredTypes, Throws.InvalidOperationException);
            }
        }

        public class InterfaceHierarchyTests : RegisterAutoMappedTypesTest
        {
            public InterfaceHierarchyTests(Type registryType)
                : base(registryType) { }

            [Test]
            public void AutoMappedClass_ImplementingInterfaceHierarchy_GetsRegisteredForAll()
            {
                var instance = _registry.Resolve<AutoDiscovery.IHierarchical2>();
                var instance2 = _registry.Resolve<AutoDiscovery.IHierarchical2>();
                var instance3 = _registry.Resolve<AutoDiscovery.IHierarchical1>();

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance, Is.Not.SameAs(instance2));

                Assert.That(instance3, Is.Not.Null);
            }

            [Test]
            public void AutoMappedClass_ComplicatedHierarchy_GetsRegisteredForAll()
            {
                var instance0 = _registry.Resolve<AutoDiscovery.IComplicatedBase>();
                var instance1 = _registry.Resolve<AutoDiscovery.IComplicated1>();
                var instance2 = _registry.Resolve<AutoDiscovery.IComplicated2>();
                var instance3 = _registry.Resolve<AutoDiscovery.IComplicated3>();
                var instance4 = _registry.Resolve<AutoDiscovery.IComplicated4>();
                var instance5 = _registry.Resolve<AutoDiscovery.IComplicated5>();

                var collection = new []
                {
                    instance0, instance1, instance2,
                    instance3, instance4, instance5
                };

                Assert.That(collection, Is.Unique);
                Assert.That(collection, Has.None.Null);
            }

            [Test]
            public void AutoMappedClass_ComplicatedHierarchyWithSingleton_GetsRegisteredForAll()
            {
                var instance0 = _registry.Resolve<AutoDiscovery.IComplicatedSingletonBase>();
                var instance1 = _registry.Resolve<AutoDiscovery.IComplicatedSingleton1>();
                var instance2 = _registry.Resolve<AutoDiscovery.IComplicatedSingleton2>();
                var instance3 = _registry.Resolve<AutoDiscovery.IComplicatedSingleton3>();
                var instance4 = _registry.Resolve<AutoDiscovery.IComplicatedSingleton4>();
                var instance5 = _registry.Resolve<AutoDiscovery.IComplicatedSingleton5>();

                var collection = new[]
                {
                    instance0, instance1, instance2,
                    instance3, instance4, instance5
                };

                Assert.That(collection, Has.None.Null);
                Assert.That(collection, Is.All.SameAs(instance0));
            }
        }
    }
}
