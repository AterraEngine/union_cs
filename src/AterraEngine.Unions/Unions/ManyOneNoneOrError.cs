// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a discriminated union that can hold one of the following types:
/// - Many, representing a collection of values of type TValue
/// - One, representing a single value of type TValue
/// - None, representing an absence of a value
/// - Error, representing an error state with a value of type TError
/// </summary>
/// <typeparam name="TValue">The type of the value(s) contained in the Many or One state.</typeparam>
/// <typeparam name="TError">The type of the value contained in the Error state.</typeparam>
public readonly partial struct ManyOneNoneOrError<TValue, TError>() : IUnion<Many<TValue>, One<TValue>, None, Error<TError>>;
