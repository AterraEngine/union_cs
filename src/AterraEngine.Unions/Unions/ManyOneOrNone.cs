// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct ManyOneOrNone<TValue>() : IUnion<Many<TValue>, One<TValue>, None> {
    public static implicit operator ManyOneOrNone<TValue>(TValue[]? value) {
        if (value is null) return new None();

        return value.Length switch {
            0 => new None(),
            1 => new One<TValue>(value[0]),
            _ => new Many<TValue>(value)
        };
    }
}
