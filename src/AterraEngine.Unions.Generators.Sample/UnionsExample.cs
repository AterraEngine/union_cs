// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace TestNamespace;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct Success<T> {
    public T Value { get; init; }
}

public readonly struct SuccessMany<T> {
    public T Values { get; init; }
}

[UnionExtra(UnionExtra.GenerateAsValue | UnionExtra.GenerateFrom)]
public readonly partial struct TupleOrFalse() : IUnion<Success<string>, SuccessMany<int[]>>;
