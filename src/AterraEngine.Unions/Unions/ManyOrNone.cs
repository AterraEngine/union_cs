// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a union type that can hold either many elements of type <typeparamref name="T"/> or none.
/// </summary>
/// <typeparam name="T">The type of elements contained in the union.</typeparam>
/// <remarks>
/// This structure is intended for scenarios where a collection of elements may be present or absent entirely.
/// The presence of elements is represented by the <see cref="Many{T}"/> structure, and the absence is represented by <see cref="None"/>.
/// </remarks>
/// <example>
/// Implicit conversion from an array allows easy creation of <see cref="ManyOrNone{T}"/> instances.
/// Null or empty arrays are converted to <see cref="None"/>, while non-empty arrays are converted to <see cref="Many{T}"/>.
/// </example>
public readonly partial struct ManyOrNone<T>() : IUnion<Many<T>, None> {
    /// <summary>
    /// Implicit conversion operator to covert an array of type <typeparamref name="T"/> to a <see cref="ManyOrNone{T}"/>.
    /// </summary>
    /// <param name="value">
    /// The array of type <typeparamref name="T"/> to convert.
    /// If the array is null, returns an instance of <see cref="None"/>.
    /// If the array is empty, also returns an instance of <see cref="None"/>.
    /// Otherwise, returns an instance of <see cref="Many{T}"/> wrapping the array.
    /// </param>
    /// <returns>
    /// A <see cref="ManyOrNone{T}"/> based on the provided array of type <typeparamref name="T"/>.
    /// </returns>
    public static implicit operator ManyOrNone<T>(T[]? value) {
        if (value is null) return new None();

        return value.Length switch {
            0 => new None(),
            _ => new Many<T>(value)
        };
    }
}
