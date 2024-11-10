// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions.Generator.Sample;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct TrueOrFalse() : IUnion<True, False> {
    public static implicit operator TrueOrFalse(bool value) => new() {
        Value = value,
        IsTrue = value,
        IsFalse = !value
    };
}

public readonly partial struct TupleOrFalse() : IUnion<(True, Success<string>), False> {
    public static implicit operator TupleOrFalse(bool value) => new() {
        Value = value,
        IsTupleOfTrueAndSuccessOfString = value,
        IsFalse = !value
    };
}