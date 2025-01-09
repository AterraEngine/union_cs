// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a discriminated union type that encapsulates one of three possible states:
/// a collection of values of type <typeparamref name="TValue" /> (Many), the absence of a value (None),
/// or an error represented by a value of type <typeparamref name="TError" /> (Error).
/// </summary>
/// <typeparam name="TValue">The type of the values contained in the Many state.</typeparam>
/// <typeparam name="TError">The type of the value representing the Error state.</typeparam>
/// <remarks>
/// This type is part of the union types pattern, offering a structured and type-safe way to
/// work with values that can take on multiple well-defined forms.
/// - The Many state is used to provide multiple values, implemented using the <see cref="Many{T}" /> type.
/// - The None state signals the absence of any values, using the <see cref="None" /> type.
/// - The Error state indicates an operation failure or invalid state, encapsulating an error value
/// using the <see cref="Error{T}" /> type.
/// </remarks>
public readonly partial struct ManyNoneOrError<TValue, TError>() : IUnion<Many<TValue>, None, Error<TError>>;

/// <summary>
/// Represents a discriminated union type that encapsulates one of three possible states:
/// a collection of values of type <typeparamref name="TValue" /> (Many), the absence of a value (None),
/// or an error represented by a value of type string.
/// </summary>
/// <typeparam name="TValue">The type of the values contained in the Many state.</typeparam>
/// <remarks>
/// This type is designed to provide a type-safe mechanism for managing multiple outcomes or results in a
/// structured manner. It is particularly useful in scenarios where a process might return multiple
/// values, no values, or an error.
/// - The Many state accommodates a collection of values using the <see cref="Many{T}" /> structure.
/// - The None state denotes absence of values using the <see cref="None" /> structure.
/// - The Error state encapsulates an error or exceptional scenario using the <see cref="Error{T}" /> structure.
/// </remarks>
public readonly partial struct ManyNoneOrError<TValue>() : IUnion<Many<TValue>, None, Error<string>>;
