// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a discriminated union that encapsulates either a <see cref="True"/> or a <see cref="False"/> value.
/// </summary>
/// <remarks>
/// This struct provides implicit conversions between boolean values and the union types:
/// - A `true` boolean value converts to an instance of <see cref="True"/>.
/// - A `false` boolean value converts to an instance of <see cref="False"/>.
/// Similarly, instances of the types convert to their corresponding boolean values:
/// - <see cref="True"/> evaluates to `true`.
/// - <see cref="False"/> evaluates to `false`.
/// The struct is immutable and complies with the <see cref="IUnion{T0, T1}"/> interface to ensure proper union behavior.
/// </remarks>
public readonly partial struct TrueOrFalse() : IUnion<True, False> {
    /// <summary>
    /// Allows an implicit conversion of a boolean value to the <see cref="TrueOrFalse"/> type.
    /// </summary>
    /// <param name="value">The boolean value to be converted. A `true` value will result in an instance of <see cref="True"/>, and a `false` value will result in an instance of <see cref="False"/>.</param>
    /// <returns>
    /// An instance of <see cref="TrueOrFalse"/> representing the boolean value.
    /// </returns>
    public static implicit operator TrueOrFalse(bool value) => value ? new True() : new False();
    /// <summary>
    /// Defines an implicit conversion operator for converting a <see cref="TrueOrFalse"/> struct to a boolean value.
    /// </summary>
    /// <param name="trueOrFalse">The <see cref="TrueOrFalse"/> instance that needs to be converted.</param>
    /// <returns>
    /// Returns <c>true</c> if the <see cref="TrueOrFalse"/> instance represents a <see cref="True"/>; otherwise, returns <c>false</c>.
    /// </returns>
    public static implicit operator bool(TrueOrFalse trueOrFalse) => trueOrFalse.IsTrue;
}
