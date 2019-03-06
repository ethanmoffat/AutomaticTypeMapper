namespace AutomaticTypeMapper.Test.Types
{
    [MappedType]
    public class BasicClass { }

    [MappedType(Tag = nameof(BasicClassTagged))]
    public class BasicClassTagged { }

    [MappedType(IsSingleton = true)]
    public class BasicClassSingleton { }

    [MappedType(IsSingleton = true, Tag = nameof(BasicClassTaggedSingleton))]
    public class BasicClassTaggedSingleton { }
}
