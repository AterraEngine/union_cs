// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace AterraEngine.Unions.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a union object structure used in the generation of union types.
/// </summary>
public readonly struct UnionObject(string structName, string nameSpace, Dictionary<ITypeSymbol, string?> typesWithAliases, ImmutableArray<string> typeParameters, bool isRecordStruct) {
    /// <summary>
    ///     Gets the name of the union struct associated with this instance.
    /// </summary>
    public string StructName { get; } = structName;

    /// <summary>
    ///     Gets the namespace associated with the UnionObject.
    /// </summary>
    public string Namespace { get; } = nameSpace;

    /// <summary>
    ///     Represents a dictionary property that maps type symbols to their optional aliases.
    /// </summary>
    public Dictionary<ITypeSymbol, string?> TypesWithAliases { get; } = typesWithAliases;

    /// <summary>
    ///     Represents the generic type parameters for a union object.
    /// </summary>
    private ImmutableArray<string> TypeParameters { get; } = typeParameters;

    /// <summary>
    ///     Indicates whether the struct declared in the union object is a record struct.
    /// </summary>
    public bool IsRecordStruct { get; } = isRecordStruct;

    /// <summary>
    ///     Retrieves the structured class name of the union object, including its type parameters if applicable.
    /// </summary>
    public string GetStructClassName() => TypeParameters.Length > 0
        ? $"{StructName}<{string.Join(", ", TypeParameters)}>"
        : StructName;
}
