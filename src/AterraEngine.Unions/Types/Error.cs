// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents an error in the system.
///     This struct is designed to encapsulate information about an error
///     and can be used as part of a result or union type to convey failure states.
/// </summary>
public readonly struct Error;

/// <summary>
///     Represents a value indicating an error. The generic parameter <typeparamref name="T" /> specifies the
///     type of the value associated with the error.
/// </summary>
/// <typeparam name="T">
///     The type of the value associated with the error.
/// </typeparam>
public readonly record struct Error<T>(T Value);
