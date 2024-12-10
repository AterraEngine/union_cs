// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a discriminated union of either a collection of elements
/// encapsulated in a <c>Some</c> instance, or an absence of elements represented by <c>None</c>.
/// </summary>
/// <typeparam name="T">
/// The type of the elements stored within the union, if present.
/// </typeparam>
/// <remarks>
/// This struct provides an implicit conversion operator to allow convenience
/// in creating instances from an array of type <c>T</c>. Specifically:
/// - If the provided array is <c>null</c>, the union is initialized to <c>None</c>.
/// - If the array is empty, the union is also initialized to <c>None</c>.
/// - If the array contains elements, the union is initialized to <c>Some</c> with the array as its value.
/// </remarks>
/// <seealso cref="Some{T}"/>
/// <seealso cref="None"/>
public readonly partial struct SomeOrNone<T>() : IUnion<Some<T>, None>{
    /// <summary>
    /// Defines an implicit conversion operator for converting a nullable array of type <typeparamref name="T"/>
    /// into a <see cref="SomeOrNone{T}"/> structure.
    /// </summary>
    /// <param name="value">The nullable array to be converted.</param>
    /// <returns>
    /// A <see cref="SomeOrNone{T}"/> instance that represents the provided array.
    /// If the array is null or empty, a `None` instance is returned. Otherwise, a `Some<T>` instance containing the array is returned.
    /// </returns>
    public static implicit operator SomeOrNone<T>(T[]? value) {
        if (value is null) return new None();

        return value.Length switch {
            0 => new None(),
            _ => new Some<T>(value)
        };
    }
}
