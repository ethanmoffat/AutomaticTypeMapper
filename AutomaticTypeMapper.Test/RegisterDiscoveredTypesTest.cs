﻿using AutomaticTypeMapper.Test.Types;
using NUnit.Framework;
using System.Linq;
using System.Reflection;

namespace AutomaticTypeMapper.Test
{
    [TestFixture]
    public class RegisterDiscoveredTypesTest
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
                var instances = _registry.ResolveAll<VariedInterface>();
                var instances2 = _registry.ResolveAll<VariedInterface>();

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

            [TearDown]
            public void TearDown()
            {
                _registry.Dispose();
            }
        }
    }
}