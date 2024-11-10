// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions.Generator.Sample;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public partial struct GenericUnion<T>() : IUnion<Success<T>, None, Error<string>> {
    public static implicit operator GenericUnion<T>(T value) => new() {
        Value = value,
        IsSuccess = true
    };
}

[UnionAliases(aliasT0: "SuccessWithValue")]
public partial struct GenericUnionWithAlias<T>() : IUnion<Success<T>, None, Error<string>> {
    public static implicit operator GenericUnionWithAlias<T>(T value) => new() {
        Value = value,
        IsSuccessWithValue = true
    };
}