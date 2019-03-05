namespace Codependent.Test.Types
{
    [CodependentType()]
    public class BasicClass { }

    [CodependentType(tag: nameof(BasicClassTagged), baseType: null, singleton: false)]
    public class BasicClassTagged { }

    [CodependentType(singleton: true, baseType: null)]
    public class BasicClassSingleton { }

    [CodependentType(singleton: true, tag: nameof(BasicClassTaggedSingleton), baseType: null)]
    public class BasicClassTaggedSingleton { }
}
