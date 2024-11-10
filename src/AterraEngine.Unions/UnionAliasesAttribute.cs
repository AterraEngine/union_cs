// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Struct)]
public class UnionAliasesAttribute : Attribute{
    public string?[] Aliases { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public UnionAliasesAttribute(params string?[] aliases) => Aliases = aliases;
    public UnionAliasesAttribute(string? aliasT0 = null, string? aliasT1 = null, string? aliasT2 = null, string? aliasT3 = null, string? aliasT4 = null, string? aliasT5 = null, string? aliasT6 = null, string? aliasT7 = null, string? aliasT8 = null, string? aliasT9 = null, string? aliasT10 = null, string? aliasT11 = null, string? aliasT12 = null, string? aliasT13 = null, string? aliasT14 = null, string? aliasT15 = null) => Aliases = [aliasT0, aliasT1, aliasT2, aliasT3, aliasT4, aliasT5, aliasT6, aliasT7, aliasT8, aliasT9, aliasT10, aliasT11, aliasT12, aliasT13, aliasT14, aliasT15];
}
