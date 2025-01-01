// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a discriminated union type with four possible cases:
///     - Some: Encapsulates a collection of values of a specified type.
///     - One: Represents a single value.
///     - None: Signifies the absence of any value.
///     - Error: Encapsulates an error with a specific type.
/// </summary>
/// <typeparam name="TValue">The type of the value used in the Some and One cases.</typeparam>
/// <typeparam name="TError">The type of the associated error in the Error case.</typeparam>
/// <remarks>
///     This structure is designed to unify multiple potential states in a single type,
///     providing a flexible approach to handle presence, absence, or errors in data or operations.
/// </remarks>
public readonly partial struct SomeOneNoneOrError<TValue, TError>() : IUnion<Some<TValue>, One<TValue>, None, Error<TError>>;
