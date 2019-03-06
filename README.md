## About
AutomaticTypeMapper provides automatic mapping of types in an IoC container for C# projects. Types tagged with a `MappedType` attribute are discovered by the type registrar, which is a wrapper around an IoC container (currently only unity).

## Build Status

[![Build status](https://ethanmoffat.visualstudio.com/EndlessClient/_apis/build/status/AutomaticTypeMapper%20PR%20Gate)](https://ethanmoffat.visualstudio.com/EndlessClient/_build/latest?definitionId=5)

## Usage

```C#

[MappedType]
class TestClass { }

var assemblyName = Assembly.GetExecutingAssembly().FullName;
using (var registry = new UnityRegistry(assemblyName))
{
    registry.RegisterDiscoveredTypes();
    var classInstance = registry.Resolve<TestClass>();
}
```
