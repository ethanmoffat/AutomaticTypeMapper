namespace Codependent.Test.Types
{
    public interface VariedInterface { }

    [CodependentType(BaseType = typeof(VariedInterface))]
    public class VariedInterfaceImplementation1 : VariedInterface
    {
    }

    [CodependentType(BaseType = typeof(VariedInterface))]
    public class VariedInterfaceImplementation2 : VariedInterface
    {
    }

    [CodependentType(BaseType = typeof(VariedInterface))]
    public class VariedInterfaceImplementation3 : VariedInterface
    {
    }

    [CodependentType(BaseType = typeof(VariedInterface), IsSingleton = true)]
    public class VariedInterfaceImplementationSingleton1 : VariedInterface
    {
    }

    [CodependentType(BaseType = typeof(VariedInterface), IsSingleton = true)]
    public class VariedInterfaceImplementationSingleton2 : VariedInterface
    {
    }

    [CodependentType(BaseType = typeof(VariedInterface), IsSingleton = true)]
    public class VariedInterfaceImplementationSingleton3 : VariedInterface
    {
    }
}
