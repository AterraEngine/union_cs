// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a value that is always evaluated as <c>false</c>.
/// </summary>
/// <remarks>
///     This struct is used in conjunction with constructs like <see cref="True" /> to represent
///     boolean-like discriminated unions. It provides an implicit conversion to a boolean
///     value, always returning <c>false</c>.
/// </remarks>
public readonly struct False {
    // ReSharper disable once UnusedParameter.Global
    /// <summary>
    ///     Represents a readonly struct that always implicitly converts to a boolean value of <c>false</c>.
    /// </summary>
    /// <remarks>
    ///     This struct overloads the implicit conversion operator to ensure that any instance of <see cref="False" />
    ///     will evaluate to <c>false</c> when used in a boolean context.
    /// </remarks>
    public static implicit operator bool(False _) => false;
}
