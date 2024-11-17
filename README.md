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

#### Normal benchmarks:
| Method                                                |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|-------------------------------------------------------|-----------:|----------:|----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| AterraEngineUnions_UnionT8_SwitchCase_Value           |  0.0084 ns | 0.0130 ns | 0.0109 ns |  0.0035 ns | 0.002 |    0.00 |      - |         - |          NA |
| AterraEngineUnions_UnionT8_TryGetAs                   |  0.0498 ns | 0.0429 ns | 0.0401 ns |  0.0512 ns | 0.009 |    0.01 |      - |         - |          NA |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Struct |  0.1706 ns | 0.0034 ns | 0.0030 ns |  0.1711 ns | 0.031 |    0.00 |      - |         - |          NA |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Value  |  4.3034 ns | 0.0903 ns | 0.0845 ns |  4.3041 ns | 0.791 |    0.02 | 0.0014 |      24 B |          NA |
| AterraEngineUnions_TrueFalse_TryGetAsTrue             |  5.4398 ns | 0.0168 ns | 0.0157 ns |  5.4353 ns | 1.000 |    0.00 |      - |         - |          NA |
| OneOf_SuccessOrFailure_SwitchCase_Value               |  6.6901 ns | 0.1500 ns | 0.1403 ns |  6.6654 ns | 1.230 |    0.03 | 0.0014 |      24 B |          NA |
| OneOfTrueFalse_TryGetAsTrue                           |  9.8679 ns | 0.2493 ns | 0.5779 ns |  9.7943 ns | 1.814 |    0.11 | 0.0038 |      64 B |          NA |
| OneOf_OneOfT8_SwitchCase_Value                        | 10.5042 ns | 0.2584 ns | 0.2291 ns | 10.4647 ns | 1.931 |    0.04 | 0.0038 |      64 B |          NA |
| OneOf_OneOfT8_TryGetAs                                | 14.1214 ns | 0.1989 ns | 0.1860 ns | 14.0284 ns | 2.596 |    0.03 | 0.0038 |      64 B |          NA |
| Dunet_TrueFalse_MatchTrue                             | 25.9659 ns | 0.5343 ns | 0.4998 ns | 25.9803 ns | 4.773 |    0.09 | 0.0105 |     176 B |          NA |

#### Enhanced benchmarks
| Method                                               |      Mean |     Error |    StdDev |   Gen0 | Allocated |
|------------------------------------------------------|----------:|----------:|----------:|-------:|----------:|
| AterraEngineUnions_UnionT8_SwitchCase_Value_Enhanced |  8.013 ns | 0.0674 ns | 0.0563 ns |      - |         - |
| AterraEngineUnions_UnionT8_TryGetAs_Enhanced         | 15.455 ns | 0.2575 ns | 0.2529 ns |      - |         - |
| OneOf_OneOfT8_SwitchCase_Value_Enhanced              | 19.011 ns | 0.2416 ns | 0.2260 ns | 0.0038 |      64 B |
| OneOf_OneOfT8_TryGetAs_Enhanced                      | 29.328 ns | 0.2134 ns | 0.1996 ns | 0.0038 |      64 B |
