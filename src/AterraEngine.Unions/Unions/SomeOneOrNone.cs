// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a union type that encapsulates one of the following possible states:
///     - A collection of values (Some)
///     - A single value (One)
///     - No value (None)
/// </summary>
/// <typeparam name="T">The type of the encapsulated value(s).</typeparam>
/// <remarks>
///     This struct allows implicit conversion from an array of type <typeparamref name="T" />.
///     Depending on the state of the array, it will return:
///     - <see cref="None" /> if the array is null or empty.
///     - <see cref="One{T}" /> if the array contains a single element.
///     - <see cref="Some{T}" /> if the array contains more than one element.
/// </remarks>
public readonly partial struct SomeOneOrNone<T>() : IUnion<Some<T>, One<T>, None> {
    /// <summary>
    ///     Provides an implicit conversion from an array of type <typeparamref name="T" /> to a
    ///     <see cref="SomeOneOrNone{T}" /> union.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="value">The array to be converted.</param>
    /// <returns>
    ///     Returns a <see cref="None" /> instance if the array is null or empty,
    ///     a <see cref="One{T}" /> instance if the array contains exactly one element,
    ///     or a <see cref="Some{T}" /> instance for arrays containing more than one element.
    /// </returns>
    public static implicit operator SomeOneOrNone<T>(T[]? value) {
        if (value is null) return new None();

        return value.Length switch {
            0 => new None(),
            1 => new One<T>(value[0]),
            _ => new Some<T>(value)
        };
    }
}
