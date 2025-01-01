// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a value of `true` in a strongly-typed manner.
/// </summary>
/// <remarks>
///     This struct provides an implicit conversion to the boolean `true` value.
///     It is primarily used in scenarios where a distinct type representation of `true` is needed,
///     such as in discriminated unions or other type-safe paradigms.
/// </remarks>
public readonly struct True {
    // ReSharper disable once UnusedParameter.Global
    /// <summary>
    ///     Defines an implicit conversion operator to convert the <see cref="True" /> struct to a boolean value.
    /// </summary>
    /// <param name="_">An instance of the <see cref="True" /> struct.</param>
    /// <returns>Returns <c>true</c> when an instance of <see cref="True" /> is converted to a boolean.</returns>
    public static implicit operator bool(True _) => true;
}
