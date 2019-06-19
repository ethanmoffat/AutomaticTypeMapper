using WorkingAssembly;

namespace AutomaticTypeMapper.Test.Types
{
    public interface BaseInterface1 { }
    public interface BaseInterface2 { }

    [MappedType(BaseType = typeof(BaseInterface1))]
    [MappedType(BaseType = typeof(BaseInterface2))]
    public class MultipleAttributes : BaseInterface1, BaseInterface2 { }

    public interface BaseInterfaceSingleton1 { }
    public interface BaseInterfaceSingleton2 { }

    [MappedType(BaseType = typeof(BaseInterfaceSingleton1), IsSingleton = true)]
    [MappedType(BaseType = typeof(BaseInterfaceSingleton2), IsSingleton = true)]
    public class MultipleAttributesSingleton : BaseInterfaceSingleton1, BaseInterfaceSingleton2 { }

    public interface NotAcrossAssembly { }

    [MappedType(BaseType = typeof(NotAcrossAssembly))]
    [MappedType(BaseType = typeof(UsedAcrossAssemblies))]
    public class UsedAcrossAssembliesImpl2 : UsedAcrossAssemblies, NotAcrossAssembly { }
}
