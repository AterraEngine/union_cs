// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct ManyNoneOrError<TValue, TError>() : IUnion<Many<TValue>, None, Error<TError>> {
    private static ManyNoneOrError<TValue, TError> FromEnumerable(IEnumerable<TValue> value) {
        IEnumerable<TValue> enumerable = value as TValue[] ?? value.ToArray();
        return !enumerable.Any() ? new None() : new Many<TValue>(enumerable);
    }
        
    public static implicit operator ManyNoneOrError<TValue, TError>(TValue[] values) => FromEnumerable(values);
    public static implicit operator ManyNoneOrError<TValue, TError>(List<TValue> values) => FromEnumerable(values);
    public static implicit operator ManyNoneOrError<TValue, TError>(HashSet<TValue> values) => FromEnumerable(values);
}
