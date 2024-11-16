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

| Method                                                |      Mean |     Error |    StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|-------------------------------------------------------|----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Struct |  2.341 ns | 0.0718 ns | 0.1384 ns |  0.31 |    0.02 | 0.0014 |      24 B |        1.00 |
| AterraEngineUnions_SuccessOrFailure_SwitchCase_Value  |  3.440 ns | 0.0536 ns | 0.0448 ns |  0.46 |    0.01 | 0.0014 |      24 B |        1.00 |
| AterraEngineUnions_UnionT8_TryGetAs                   |  4.888 ns | 0.0106 ns | 0.0089 ns |  0.65 |    0.01 |      - |         - |        0.00 |
| OneOf_SuccessOrFailure_SwitchCase_Value               |  5.573 ns | 0.0499 ns | 0.0467 ns |  0.74 |    0.01 | 0.0014 |      24 B |        1.00 |
| AterraEngineUnions_UnionT8_SwitchCase_Value           |  7.262 ns | 0.0039 ns | 0.0034 ns |  0.96 |    0.01 |      - |         - |        0.00 |
| AterraEngineUnions_TrueFalse_TryGetAsTrue             |  7.544 ns | 0.0894 ns | 0.0792 ns |  1.00 |    0.01 | 0.0014 |      24 B |        1.00 |
| OneOfTrueFalse_TryGetAsTrue                           |  7.810 ns | 0.1197 ns | 0.1000 ns |  1.04 |    0.02 | 0.0038 |      64 B |        2.67 |
| OneOf_OneOfT8_SwitchCase_Value                        |  8.746 ns | 0.0884 ns | 0.0738 ns |  1.16 |    0.02 | 0.0038 |      64 B |        2.67 |
| OneOf_OneOfT8_TryGetAs                                | 11.956 ns | 0.1835 ns | 0.1627 ns |  1.58 |    0.03 | 0.0038 |      64 B |        2.67 |
| Dunet_TrueFalse_MatchTrue                             | 21.764 ns | 0.1710 ns | 0.1428 ns |  2.89 |    0.03 | 0.0105 |     176 B |        7.33 |
