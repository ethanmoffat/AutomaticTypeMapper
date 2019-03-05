namespace Codependent.Test.Types
{
    public interface BasicInterface { }

    [CodependentType(typeof(BasicInterface))]
    public class InterfaceImplementation : BasicInterface { }

    public interface BasicInterfaceTagged { }

    [CodependentType(typeof(BasicInterfaceTagged), Tag = nameof(InterfaceImplementationTagged))]
    public class InterfaceImplementationTagged : BasicInterfaceTagged { }

    public interface BasicInterfaceSingleton { }

    [CodependentType(typeof(BasicInterfaceSingleton), IsSingleton = true)]
    public class InterfaceImplementationSingleton : BasicInterfaceSingleton { }

    public interface BasicInterfaceTaggedSingleton { }

    [CodependentType(typeof(BasicInterfaceTaggedSingleton), Tag = nameof(InterfaceImplementationTaggedSingleton), IsSingleton = true)]
    public class InterfaceImplementationTaggedSingleton : BasicInterfaceTaggedSingleton { }
}
