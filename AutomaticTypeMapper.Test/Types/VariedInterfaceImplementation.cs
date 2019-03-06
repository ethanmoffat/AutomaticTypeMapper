namespace AutomaticTypeMapper.Test.Types
{
    public interface VariedInterface { }

    [MappedType(BaseType = typeof(VariedInterface))]
    public class VariedInterfaceImplementation1 : VariedInterface
    {
    }

    [MappedType(BaseType = typeof(VariedInterface))]
    public class VariedInterfaceImplementation2 : VariedInterface
    {
    }

    [MappedType(BaseType = typeof(VariedInterface))]
    public class VariedInterfaceImplementation3 : VariedInterface
    {
    }

    [MappedType(BaseType = typeof(VariedInterface), IsSingleton = true)]
    public class VariedInterfaceImplementationSingleton1 : VariedInterface
    {
    }

    [MappedType(BaseType = typeof(VariedInterface), IsSingleton = true)]
    public class VariedInterfaceImplementationSingleton2 : VariedInterface
    {
    }

    [MappedType(BaseType = typeof(VariedInterface), IsSingleton = true)]
    public class VariedInterfaceImplementationSingleton3 : VariedInterface
    {
    }
}
