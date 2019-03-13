using System;

namespace AutomaticTypeMapper.Test.Types
{
    public class AutoDiscovery
    {
        #region Basic Class

        [AutoMappedType]
        public class BasicClass { }

        [AutoMappedType(Tag = nameof(BasicClassTagged))]
        public class BasicClassTagged { }

        [AutoMappedType(IsSingleton = true)]
        public class BasicClassSingleton { }

        [AutoMappedType(IsSingleton = true, Tag = nameof(BasicClassTaggedSingleton))]
        public class BasicClassTaggedSingleton { }

        #endregion

        #region Basic Interface

        public interface BasicInterface { }

        [AutoMappedType]
        public class InterfaceImplementation : BasicInterface { }

        public interface BasicInterfaceTagged { }

        [AutoMappedType(Tag = nameof(InterfaceImplementationTagged))]
        public class InterfaceImplementationTagged : BasicInterfaceTagged { }

        public interface BasicInterfaceSingleton { }

        [AutoMappedType(IsSingleton = true)]
        public class InterfaceImplementationSingleton : BasicInterfaceSingleton { }

        public interface BasicInterfaceTaggedSingleton { }

        [AutoMappedType(Tag = nameof(InterfaceImplementationTaggedSingleton), IsSingleton = true)]
        public class InterfaceImplementationTaggedSingleton : BasicInterfaceTaggedSingleton { }

        #endregion

        #region Multiple Interfaces

        public interface BaseInterface1 { }
        public interface BaseInterface2 { }

        [AutoMappedType]
        public class MultipleAttributes : BaseInterface1, BaseInterface2 { }

        public interface BaseInterfaceSingleton1 { }
        public interface BaseInterfaceSingleton2 { }

        [AutoMappedType(IsSingleton = true)]
        public class MultipleAttributesSingleton : BaseInterfaceSingleton1, BaseInterfaceSingleton2 { }

        #endregion

        #region Varied Interace Implementations

        public interface VariedInterface { }

        [AutoMappedType]
        public class VariedInterfaceImplementation1 : VariedInterface { }

        [AutoMappedType]
        public class VariedInterfaceImplementation2 : VariedInterface { }

        [AutoMappedType]
        public class VariedInterfaceImplementation3 : VariedInterface { }

        [AutoMappedType(IsSingleton = true)]
        public class VariedInterfaceImplementationSingleton1 : VariedInterface { }

        [AutoMappedType(IsSingleton = true)]
        public class VariedInterfaceImplementationSingleton2 : VariedInterface { }

        [AutoMappedType(IsSingleton = true)]
        public class VariedInterfaceImplementationSingleton3 : VariedInterface { }

        #endregion

        #region Interface Hierarchy / Complicated cases

        public interface IHierarchical1 { }
        public interface IHierarchical2 : IHierarchical1 { }

        [AutoMappedType]
        public class HierarchicalImplementation : IHierarchical2 { }

        public interface IComplicatedBase { }
        public interface IComplicated1 : IComplicatedBase { }
        public interface IComplicated2 : IComplicatedBase { }
        public interface IComplicated3 : IComplicated1 { }
        public interface IComplicated4 : IComplicated2 { }
        public interface IComplicated5 : IComplicatedBase { }

        [AutoMappedType]
        public class ComplicatedClass : IComplicated3, IComplicated4, IComplicated5 { }

        public interface IComplicatedSingletonBase { }
        public interface IComplicatedSingleton1 : IComplicatedSingletonBase { }
        public interface IComplicatedSingleton2 : IComplicatedSingletonBase { }
        public interface IComplicatedSingleton3 : IComplicatedSingleton1 { }
        public interface IComplicatedSingleton4 : IComplicatedSingleton2 { }
        public interface IComplicatedSingleton5 : IComplicatedSingletonBase { }

        [AutoMappedType(IsSingleton = true)]
        public class ComplicatedSingletonClass : IComplicatedSingleton3, IComplicatedSingleton4, IComplicatedSingleton5 { }

        #endregion
    }
}
