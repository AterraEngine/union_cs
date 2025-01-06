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
public readonly struct UnionObject(string structName, string nameSpace, Dictionary<ITypeSymbol, string?> typesWithAliases, ImmutableArray<string> typeParameters, bool isRecordStruct, int extraGeneratorFlags) {
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

    public bool HasFlagGenerateFrom() =>    (extraGeneratorFlags & 0b1) != 0;
    public bool HasFlagGenerateAsValue() => (extraGeneratorFlags & 0b10) != 0;

    public bool IsValidGenerateAsValue(ITypeSymbol typeSymbol, out bool isValues, out string valueTypeName, out string notNullWhen, out string nullable) {
        // Check if the typeSymbol inherits from IValue<T> or IValues<T>
        isValues = false;
        valueTypeName = string.Empty;
        notNullWhen = string.Empty;
        nullable = string.Empty;
        
        if (typeSymbol.AllInterfaces.IsEmpty) return false;
        foreach (INamedTypeSymbol? @interface in typeSymbol.AllInterfaces) {
            string name = @interface.ConstructedFrom.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            switch (name) {
                case "global::AterraEngine.Unions.IValue<T>": {
                    isValues = false; 
                    break;
                }

                case "global::AterraEngine.Unions.IValues<T>": {
                    isValues = true;
                    break;
                }
                default: continue;
            }
            
            valueTypeName = @interface.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            notNullWhen = @interface.TypeArguments[0].IsReferenceType ? "[NotNullWhen(true)] " : notNullWhen;
            nullable = @interface.TypeArguments[0].IsReferenceType ? "?" : nullable;
            return true;
        }
        return false;
    }
}
