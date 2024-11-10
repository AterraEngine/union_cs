// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace AterraEngine.Unions.Generator;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionObject(string structName, string nameSpace, Dictionary<ITypeSymbol, string?> typesWithAliases) {
    public string StructName { get; } = structName;
    public string Namespace { get; } = nameSpace;
    public Dictionary<ITypeSymbol, string?> TypesWithAliases { get; } = typesWithAliases;
}
