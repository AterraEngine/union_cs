// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a failure state in a computation or operation, encapsulating a value of a specific type.
/// </summary>
/// <typeparam name="T">
///     The type of the value encapsulated in the failure state.
/// </typeparam>
public readonly struct Failure;

/// <summary>
///     Represents a failure, typically used within union types or result-based patterns.
/// </summary>
/// <typeparam name="T">The type of the value that describes the failure.</typeparam>
public readonly record struct Failure<T>(T Value);
