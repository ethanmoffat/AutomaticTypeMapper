using Codependent.Test.Types;
using NUnit.Framework;
using System.Reflection;

namespace Codependent.Test
{
    [TestFixture]
    public class UnityRegistryTest
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

                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance2);
                Assert.AreNotSame(instance, instance2); //not singleton
            }

            [Test]
            public void TaggedClass_WithTag_IsRegistered()
            {
                var instance = _registry.Resolve<BasicClassTagged>(nameof(BasicClassTagged));
                var instance2 = _registry.Resolve<BasicClassTagged>(nameof(BasicClassTagged));

                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance2);
                Assert.AreNotSame(instance, instance2); //not singleton
            }

            [Test]
            public void TaggedClass_Singleton_IsRegistered()
            {
                var instance = _registry.Resolve<BasicClassSingleton>();
                var instance2 = _registry.Resolve<BasicClassSingleton>();

                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance2);
                Assert.AreSame(instance, instance2); //singleton
            }

            [Test]
            public void TaggedClass_SingletonWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<BasicClassTaggedSingleton>(nameof(BasicClassTaggedSingleton));
                var instance2 = _registry.Resolve<BasicClassTaggedSingleton>(nameof(BasicClassTaggedSingleton));

                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance2);
                Assert.AreSame(instance, instance2); //singleton
            }

            [TearDown]
            public void TearDown()
            {
                //todo: dispose the container
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

                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance2);
                Assert.AreNotSame(instance, instance2); //not singleton
            }

            [Test]
            public void TaggedClass_ImplementsInterfaceWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<BasicInterfaceTagged>(nameof(InterfaceImplementationTagged));
                var instance2 = _registry.Resolve<BasicInterfaceTagged>(nameof(InterfaceImplementationTagged));

                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance2);
                Assert.AreNotSame(instance, instance2); //not singleton
            }

            [Test]
            public void TaggedClass_ImplementsInterfaceWithSingleton_IsRegistered()
            {
                var instance = _registry.Resolve<BasicInterfaceSingleton>();
                var instance2 = _registry.Resolve<BasicInterfaceSingleton>();

                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance2);
                Assert.AreSame(instance, instance2); //singleton
            }

            [Test]
            public void TaggedClass_ImplementsInterfaceWithSingletonWithTag_IsRegistered()
            {
                var instance = _registry.Resolve<BasicInterfaceTaggedSingleton>(nameof(InterfaceImplementationTaggedSingleton));
                var instance2 = _registry.Resolve<BasicInterfaceTaggedSingleton>(nameof(InterfaceImplementationTaggedSingleton));

                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance2);
                Assert.AreSame(instance, instance2); //singleton
            }

            [TearDown]
            public void TearDown()
            {
                //todo: dispose the container
            }
        }
    }
}
