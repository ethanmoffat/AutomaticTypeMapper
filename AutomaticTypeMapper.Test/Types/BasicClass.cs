namespace AutomaticTypeMapper.Test.Types
{
    [CodependentType]
    public class BasicClass { }

    [CodependentType(Tag = nameof(BasicClassTagged))]
    public class BasicClassTagged { }

    [CodependentType(IsSingleton = true)]
    public class BasicClassSingleton { }

    [CodependentType(IsSingleton = true, Tag = nameof(BasicClassTaggedSingleton))]
    public class BasicClassTaggedSingleton { }
}
