// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions.Generator.Sample;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public partial struct GenericUnion<T>() : IUnion<Success<T>, None, Error<string>> {
    public static implicit operator GenericUnion<T>(T value) => new Success<T>(value);
}

[UnionAliases(aliasT0: "SuccessWithValue")]
public partial struct GenericUnionWithAlias<T>() : IUnion<Success<T>, None, Error<string>> {
    public static implicit operator GenericUnionWithAlias<T>(T value) => new Success<T>(value);
}