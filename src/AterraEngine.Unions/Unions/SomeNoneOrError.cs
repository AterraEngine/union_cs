// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     The SomeNoneOrError struct represents a value that can be in one of three states:
///     - Some: Contains a value of type <typeparamref name="TValue" />.
///     - None: Represents the absence of a value.
///     - Error: Contains an error of type <typeparamref name="TError" />.
///     This is a functional programming inspired union type that allows handling of success, absence, and error cases
///     in a type-safe manner, making it useful for scenarios where these three states need to be modeled.
/// </summary>
/// <typeparam name="TValue">The type encapsulated by the Some state, representing a successful value.</typeparam>
/// <typeparam name="TError">The type encapsulated by the Error state, representing an error case.</typeparam>
public readonly partial struct SomeNoneOrError<TValue, TError>() : IUnion<Some<TValue>, None, Error<TError>>;
