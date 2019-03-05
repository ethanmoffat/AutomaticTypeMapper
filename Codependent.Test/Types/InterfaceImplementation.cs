namespace Codependent.Test.Types
{
    public interface BasicInterface { }

    [CodependentType(typeof(BasicInterface))]
    public class InterfaceImplementation : BasicInterface { }

    public interface BasicInterfaceTagged { }

    [CodependentType(typeof(BasicInterfaceTagged), tag: nameof(InterfaceImplementationTagged), singleton: false)]
    public class InterfaceImplementationTagged : BasicInterfaceTagged { }

    public interface BasicInterfaceSingleton { }

    [CodependentType(typeof(BasicInterfaceSingleton), singleton: true)]
    public class InterfaceImplementationSingleton : BasicInterfaceSingleton { }

    public interface BasicInterfaceTaggedSingleton { }

    [CodependentType(typeof(BasicInterfaceTaggedSingleton), tag: nameof(InterfaceImplementationTaggedSingleton), singleton: true)]
    public class InterfaceImplementationTaggedSingleton : BasicInterfaceTaggedSingleton { }
}
