// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a collection of optional values, allowing for zero or more values to be encapsulated.
///     This is part of the AterraEngine.Unions namespace and is used to express cases where multiple optional values
///     of a specific type might be present.
/// </summary>
/// <typeparam name="T">
///     The type of values stored within the collection, which can include null values.
/// </typeparam>
public readonly record struct Some<T>(IEnumerable<T?> Values) : IValues<IEnumerable<T?>> ;
