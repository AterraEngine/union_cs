// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a discriminated union that encapsulates one of the following states:
/// - A collection of values represented by Many
/// - A single value represented by One
/// - An absence of value represented by None
/// - An error state represented by Error
/// </summary>
/// <typeparam name="TValue">The type of the value(s) associated with the Many and One states.</typeparam>
/// <typeparam name="TError">The type of the value associated with the Error state.</typeparam>
public readonly partial struct ManyOneNoneOrError<TValue, TError>() : IUnion<Many<TValue>, One<TValue>, None, Error<TError>>;

/// <summary>
/// Represents a discriminated union capable of holding one of the following states:
/// - A collection of values, represented by the Many state containing elements of type TValue.
/// - A single value, represented by the One state containing a value of type TValue.
/// - An absence of value, represented by the None state.
/// - An error state, represented by the Error state containing a value of type TError.
/// </summary>
/// <typeparam name="TValue">The type of value(s) contained in the Many or One states.</typeparam>
/// <typeparam name="TError">The type of the error value contained in the Error state.</typeparam>
public readonly partial struct ManyOneNoneOrError<TValue>() : IUnion<Many<TValue>, One<TValue>, None, Error<string>>;
