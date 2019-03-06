namespace AutomaticTypeMapper.Test.Types
{
    public interface BasicInterface { }

    [MappedType(typeof(BasicInterface))]
    public class InterfaceImplementation : BasicInterface { }

    public interface BasicInterfaceTagged { }

    [MappedType(typeof(BasicInterfaceTagged), Tag = nameof(InterfaceImplementationTagged))]
    public class InterfaceImplementationTagged : BasicInterfaceTagged { }

    public interface BasicInterfaceSingleton { }

    [MappedType(typeof(BasicInterfaceSingleton), IsSingleton = true)]
    public class InterfaceImplementationSingleton : BasicInterfaceSingleton { }

    public interface BasicInterfaceTaggedSingleton { }

    [MappedType(typeof(BasicInterfaceTaggedSingleton), Tag = nameof(InterfaceImplementationTaggedSingleton), IsSingleton = true)]
    public class InterfaceImplementationTaggedSingleton : BasicInterfaceTaggedSingleton { }
}
