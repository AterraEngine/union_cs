// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a readonly structure that encapsulates a collection of elements of a specified type.
/// </summary>
/// <typeparam name="T">
///     The type of elements contained within the structure.
/// </typeparam>
public readonly struct Many<T>(IEnumerable<T> values) : IValues<T[]> {
    /// <summary>
    ///     An array of type T that contains the values provided to the Many struct.
    ///     Represents the internal storage of elements ensuring efficient access and manipulation.
    /// </summary>
    public T[] Values { get; } =  values as T[] ?? values.ToArray();
}
