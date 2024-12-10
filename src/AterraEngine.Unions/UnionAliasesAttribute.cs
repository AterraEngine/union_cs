// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Specifies custom alias names for the generic type parameters of a union struct.
/// </summary>
/// <remarks>
/// This attribute is used to assign meaningful alias names to the individual type arguments
/// in generic union structs. The aliases can provide clarity in the code when working with unions
/// that encapsulate multiple types. Names specified in this attribute correspond to the type parameters
/// (T0, T1, ..., T15) in the order they appear in the union struct.
/// </remarks>
/// <param name="aliasT0">Optional alias for type parameter T0.</param>
/// <param name="aliasT1">Optional alias for type parameter T1.</param>
/// <param name="aliasT2">Optional alias for type parameter T2.</param>
/// <param name="aliasT3">Optional alias for type parameter T3.</param>
/// <param name="aliasT4">Optional alias for type parameter T4.</param>
/// <param name="aliasT5">Optional alias for type parameter T5.</param>
/// <param name="aliasT6">Optional alias for type parameter T6.</param>
/// <param name="aliasT7">Optional alias for type parameter T7.</param>
/// <param name="aliasT8">Optional alias for type parameter T8.</param>
/// <param name="aliasT9">Optional alias for type parameter T9.</param>
/// <param name="aliasT10">Optional alias for type parameter T10.</param>
/// <param name="aliasT11">Optional alias for type parameter T11.</param>
/// <param name="aliasT12">Optional alias for type parameter T12.</param>
/// <param name="aliasT13">Optional alias for type parameter T13.</param>
/// <param name="aliasT14">Optional alias for type parameter T14.</param>
/// <param name="aliasT15">Optional alias for type parameter T15.</param>
[AttributeUsage(AttributeTargets.Struct)]
public class UnionAliasesAttribute(
    string? aliasT0 = null,
    string? aliasT1 = null,
    string? aliasT2 = null,
    string? aliasT3 = null,
    string? aliasT4 = null,
    string? aliasT5 = null,
    string? aliasT6 = null,
    string? aliasT7 = null,
    string? aliasT8 = null,
    string? aliasT9 = null,
    string? aliasT10 = null,
    string? aliasT11 = null,
    string? aliasT12 = null,
    string? aliasT13 = null,
    string? aliasT14 = null,
    string? aliasT15 = null
) : Attribute {
    /// Gets an array of alias strings provided to the UnionAliasesAttribute.
    /// Each alias corresponds to one type parameter of a union type.
    /// If no alias is provided for a specific type parameter, the value will be null.
    public string?[] Aliases { get; } = [
        aliasT0,
        aliasT1,
        aliasT2,
        aliasT3,
        aliasT4,
        aliasT5,
        aliasT6,
        aliasT7,
        aliasT8,
        aliasT9,
        aliasT10,
        aliasT11,
        aliasT12,
        aliasT13,
        aliasT14,
        aliasT15
    ];
    
}
