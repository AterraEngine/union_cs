// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a discriminated union type that encapsulates one of three possible states:
///     a collection of values of type <typeparamref name="TValue" /> (Many), the absence of a value (None),
///     or an error represented by a value of type <typeparamref name="TError" /> (Error).
/// </summary>
/// <typeparam name="TValue">The type of the values contained in the Many state.</typeparam>
/// <typeparam name="TError">The type of the value representing the Error state.</typeparam>
/// <remarks>
///     This type is part of the union types pattern, offering a structured and type-safe way to
///     work with values that can take on multiple well-defined forms.
///     - The Many state is used to provide multiple values, implemented using the <see cref="Many{T}" /> type.
///     - The None state signals the absence of any values, using the <see cref="None" /> type.
///     - The Error state indicates an operation failure or invalid state, encapsulating an error value
///     using the <see cref="Error{T}" /> type.
/// </remarks>
public readonly partial struct ManyNoneOrError<TValue, TError>() : IUnion<Many<TValue>, None, Error<TError>>;
