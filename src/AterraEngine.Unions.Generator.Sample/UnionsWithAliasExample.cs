// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions.Generator.Sample;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases(aliasT0:"Succeeded")]
public readonly partial struct SucceededOrFalse() : IUnion<(Success<string>, None), False>;


[UnionAliases(aliasT1: "Empty")]
public readonly partial struct TupleOrEmpty() : IUnion<(True, Success<string>), False>;


[UnionAliases(aliasT0: "Nothing", aliasT1: "Something")]
public readonly partial struct NothingOrSomething() : IUnion<True, False>;

[UnionAliases(aliasT2: "Alias")]
public readonly partial struct TrueFalseOrAlias() : IUnion<True, False, None>;