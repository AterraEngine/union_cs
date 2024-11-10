// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace AterraEngine.Unions.Generator;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct UnionObject(string structName, string nameSpace, Dictionary<ITypeSymbol, string?> typesWithAliases, ImmutableArray<string> typeParameters) {
    public string StructName { get; } = structName;
    public string Namespace { get; } = nameSpace;
    public Dictionary<ITypeSymbol, string?> TypesWithAliases { get; } = typesWithAliases;
    public ImmutableArray<string> TypeParameters { get; } = typeParameters;

    public string GetStructClassName => TypeParameters.Length > 0
        ? $"{StructName}<{string.Join(", ", TypeParameters)}>"
        : StructName;
        

}