// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a success result or outcome in operational contexts.
/// </summary>
/// <remarks>
///     This struct is typically used to denote a successful operation or state
///     in scenarios where success needs to be explicitly communicated or
///     differentiated from other outcomes.
/// </remarks>
public readonly struct Success;

/// <summary>
///     Represents a successful result. It is used as a type carrier for values indicative of successful states or
///     outcomes.
/// </summary>
/// <typeparam name="T">
///     The type of the value encapsulated by the success result.
/// </typeparam>
public readonly record struct Success<T>(T Value) : IValue<T>;
