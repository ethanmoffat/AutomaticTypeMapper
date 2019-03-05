namespace Codependent.Test.Types
{
    public interface VariedInterface { }

    [CodependentType(baseType: typeof(VariedInterface))]
    public class VariedInterfaceImplementation1 : VariedInterface
    {
    }

    [CodependentType(baseType: typeof(VariedInterface))]
    public class VariedInterfaceImplementation2 : VariedInterface
    {
    }

    [CodependentType(baseType: typeof(VariedInterface))]
    public class VariedInterfaceImplementation3 : VariedInterface
    {
    }

    [CodependentType(baseType: typeof(VariedInterface), singleton: true)]
    public class VariedInterfaceImplementationSingleton1 : VariedInterface
    {
    }

    [CodependentType(baseType: typeof(VariedInterface), singleton: true)]
    public class VariedInterfaceImplementationSingleton2 : VariedInterface
    {
    }

    [CodependentType(baseType: typeof(VariedInterface), singleton: true)]
    public class VariedInterfaceImplementationSingleton3 : VariedInterface
    {
    }
}
