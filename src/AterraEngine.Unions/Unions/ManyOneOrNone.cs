// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a discriminated union that encapsulates one of three possible states:
///     multiple values (<see cref="Many{TValue}" />), a single value (<see cref="One{TValue}" />), or no value (
///     <see cref="None" />).
/// </summary>
/// <typeparam name="TValue">The type of value(s) that the union can hold.</typeparam>
/// <remarks>
///     The structure is used to seamlessly handle multiple representations of data and provides implicit conversion
///     from an array to the appropriate representation based on the content of the array:
///     - If the array is null, the structure represents <see cref="None" />.
///     - If the array has no elements, the structure represents <see cref="None" />.
///     - If the array contains a single value, the structure represents <see cref="One{TValue}" />.
///     - If the array contains multiple values, the structure represents <see cref="Many{TValue}" />.
/// </remarks>
public readonly partial struct ManyOneOrNone<TValue>() : IUnion<Many<TValue>, One<TValue>, None> {
    /// <summary>
    ///     Defines an implicit conversion operator for the <see cref="ManyOneOrNone{TValue}" /> union type.
    ///     Converts an array of type <typeparamref name="TValue" /> to a <see cref="ManyOneOrNone{TValue}" /> instance.
    /// </summary>
    /// <param name="value">
    ///     An array of type <typeparamref name="TValue" /> that will be used to create an instance of the union type.
    /// </param>
    /// <returns>
    ///     Returns a <see cref="None" /> instance if the array is null or empty.
    ///     Returns a <see cref="One{T}" /> instance if the array contains a single element.
    ///     Returns a <see cref="Many{T}" /> instance if the array contains multiple elements.
    /// </returns>
    public static implicit operator ManyOneOrNone<TValue>(TValue[]? value) {
        if (value is null) return new None();

        return value.Length switch {
            0 => new None(),
            1 => new One<TValue>(value[0]),
            _ => new Many<TValue>(value)
        };
    }
}
