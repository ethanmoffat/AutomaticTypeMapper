using AutomaticTypeMapper;

namespace InvalidTypes
{
    public interface ReusedInterface { }

    [MappedType(BaseType = typeof(ReusedInterface), IsSingleton = true)]
    [MappedType(BaseType = typeof(ReusedInterface))]
    public class MultipleSameAttributes : ReusedInterface { }
}
