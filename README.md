# 🔗 AterraEngine.Unions 🔗
A Union Library for DotNet

## Overview

`AterraEngine.Unions` is a comprehensive library for creating and managing union types in .NET.
It leverages the latest features of C# 13.0 and .NET 9.0 to provide a robust and efficient framework for representing multiple and diverse data types as a single unit.
The package was inspired by the OneOf package.

### Features

- **Type Safety**: Ensure type safety with union types that encapsulate various data forms.
- **Ease of Use**: Simplified API to integrate union types seamlessly into your project.
- **Performance Optimizations**: Designed with performance in mind to handle high-scale applications.
- **Generate**: Not satisfied with the basic unions we have made for you? No worries, you can generate your own using `AterraEngine.Unions.Generator`
- **Auto Alias**: Instead of having a pre-made `Union<T0,T1,...>` base type which simply provides a `IsT0` or `AsT1` api, this package generates all unions from the ground up. 
  - This allows us to create insert any names we want. By default, it will choose the name of the types chosen for the aliases, example : `IsTrue` `AsString`.
  - You can also set your own alias for specific types using the attribute `[UnionAliases(aliasT2:"Something")]`. See usage in the example below.
- **Up to 16**: By default the `IUnion<>` interface allows up to 16 types within the union. Because we use casting instead of the index based approach by OneOf, there doesn't need to be a limit to this in the future.

### Getting Started

#### Installation

You can install `AterraEngine.Unions` via NuGet Package Manager:
```bash
dotnet add package AterraEngine.Unions
```

You can install `AterraEngine.Unions.Generator` via NuGet Package Manager:
```bash
dotnet add package AterraEngine.Unions.Generator
```

#### Usage

Here is a basic example to demonstrate how to create and use union types with `AterraEngine.Unions`.

```csharp
using AterraEngine.Unions;

TrueOrFalse trueOrFalse = new True();

if (trueOrFalse.IsTrue) {    
    // Do stuff here
}
```

```csharp
using AterraEngine.Unions;

ManyOneNoneOrError<int, string> union = new Many<int>([1, 2, 3]);
if (union.TryGetAsMany(out Many<T> values) {
  // Do stuff here
}
if (union.TryGetAsOne(out One<T> value) {
  // Do stuff here
}
if (union.TryGetAsNone(out None value) {
  // Do stuff here
}
if (union.TryGetAsError(out Error<T> value) {
  // Do stuff here
}
```

Using `.Value` will incur boxing. If you want to avoid boxing, it is currently advised to use the `TryGetAs{TypeName}` method or a combination of `Is{TypeName}` and `As{Typename}` properties.
```csharp
using AterraEngine.Unions;

ManyOneNoneOrError<int, string> union = new One<int>(1);
switch (union.Value) {
    case Many<int>: //...
    case One<int>: //...
    case None: //...
    case Error: //...
}

if (union.TryGetAsNone(out None value) {
    // ...        
}
```
Another version of using the switch case would be like the following example.
Although a little more cumbersome to write, it will do the same as the above example, without boxing.
```csharp
TrueOrFalse union = new False();
switch (union) {
    case {IsTrue: true, AsTrue: var trueValue}: 
        Assert.Equal(new True(), trueValue);
        break;
    case {IsFalse: true, AsFalse: var falseValue}: 
        Assert.Equal(new False(), falseValue);
        break;
}
```

You can directly use the predefined `Union<>` and `Union<T0, T1, ...>` structs provided by `AterraEngine.Unions` for common use cases.
This struct has the downside of it's aliases being named `isT0`, `asT0`, etc and is this less easy to follow along what is being referenced.
```csharp
using AterraEngine.Unions;

public class UnionExample {
    public Union<string, int> GetSomeValue(bool input) {
        if (input) return "Something";
        else return 0;
    }
}
```

Creating your own unions is easily done by installing `AterraEngine.Unions.Generators` and following the example:

```csharp
using AterraEngine.Unions;

[UnionAliases(aliasT2:"ErrorTuple")]
public readonly partial struct TrueFalseOrErrorTuple() : IUnion<True, False, (Error<string>, Type)>;
// Which will generate the following, instead of a default generated name for the 3rd type in the union.
// - IsErrorTuple
// - AsErrorTuple
// - TryGetErrorTuple(...)
```

Here is an advanced example demonstrating a custom union type with user-defined aliases:

```csharp
using AterraEngine.Unions;

[UnionAliases(aliasT2: "ErrorTuple")]
public readonly partial struct TrueFalseOrErrorTuple : IUnion<True, False, (Error<string>, Type)>;

class Program {
    static void Main() {
        TrueFalseOrErrorTuple union = new True();
        
        if (union.IsTrue) {
            Console.WriteLine("It's true!");
        }

        // Using the custom alias
        union = new ((new Error<string>("An error occurred"), typeof(int)));

        if (union.IsErrorTuple) {
            var errorTuple = union.AsErrorTuple;
            Console.WriteLine($"Error: {errorTuple.Item1.Message}, Type: {errorTuple.Item2}");
        }
    }
}
```

### Benchmarks
The following is a result of the benchmarks found at [Benchmarks.AterraEngine.Unions](tests/Benchmarks.AterraEngine.Unions).
Benchmark results were last updated for version `2.4.0`

> BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4541/23H2/2023Update/SunValley3)
> AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
> .NET SDK 9.0.100
> [Host]     : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2
> DefaultJob : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2

#### Normal benchmarks:
| Method                                                |       Mean |     Error |    StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|-------------------------------------------------------|-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| AterraEngineUnions_UnionT8_TryGetAs                   |  0.0007 ns | 0.0006 ns | 0.0005 ns | 0.000 |    0.00 |      - |         - |          NA |
| AterraEngineUnions_UnionT8_SwitchCase_Value           |  0.0039 ns | 0.0040 ns | 0.0032 ns | 0.001 |    0.00 |      - |         - |          NA |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Struct |  0.0537 ns | 0.0073 ns | 0.0069 ns | 0.010 |    0.00 |      - |         - |          NA |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Value  |  3.5285 ns | 0.0187 ns | 0.0166 ns | 0.649 |    0.00 | 0.0014 |      24 B |          NA |
| AterraEngineUnions_TrueFalse_TryGetAsTrue             |  5.4344 ns | 0.0065 ns | 0.0055 ns | 1.000 |    0.00 |      - |         - |          NA |
| OneOf_SuccessOrFailure_SwitchCase_Value               |  6.0279 ns | 0.1432 ns | 0.1705 ns | 1.109 |    0.03 | 0.0014 |      24 B |          NA |
| OneOfTrueFalse_TryGetAsTrue                           |  7.5625 ns | 0.2001 ns | 0.3949 ns | 1.392 |    0.07 | 0.0038 |      64 B |          NA |
| OneOf_OneOfT8_SwitchCase_Value                        |  9.5294 ns | 0.2108 ns | 0.1760 ns | 1.754 |    0.03 | 0.0038 |      64 B |          NA |
| OneOf_OneOfT8_TryGetAs                                | 12.8456 ns | 0.1744 ns | 0.1632 ns | 2.364 |    0.03 | 0.0038 |      64 B |          NA |
| Dunet_TrueFalse_MatchTrue                             | 21.2662 ns | 0.4654 ns | 0.4125 ns | 3.913 |    0.07 | 0.0105 |     176 B |          NA |

#### Enhanced benchmarks
| Method                                               |      Mean |     Error |    StdDev |   Gen0 | Allocated |
|------------------------------------------------------|----------:|----------:|----------:|-------:|----------:|
| AterraEngineUnions_UnionT8_SwitchCase_Value_Enhanced |  7.735 ns | 0.0171 ns | 0.0143 ns |      - |         - |
| AterraEngineUnions_UnionT8_TryGetAs_Enhanced         | 14.725 ns | 0.0378 ns | 0.0335 ns |      - |         - |
| OneOf_OneOfT8_SwitchCase_Value_Enhanced              | 17.159 ns | 0.3368 ns | 0.5719 ns | 0.0038 |      64 B |
| OneOf_OneOfT8_TryGetAs_Enhanced                      | 27.468 ns | 0.2072 ns | 0.1939 ns | 0.0038 |      64 B |

