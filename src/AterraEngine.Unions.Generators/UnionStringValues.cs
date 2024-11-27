// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using System.Linq;

namespace AterraEngine.Unions.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct UnionStringValues(ITypeSymbol type, string? alias) {
    public readonly string Type = type.ToString();
    public readonly string Alias = alias ?? GetTypeAlias(type);
    public string IsAlias => $"Is{Alias}";
    public string AsAlias => $"As{Alias}";
    public string TypeNullable => type.IsReferenceType ? "?" : string.Empty;
    public string NotNullWhen => type.IsReferenceType ? "[NotNullWhen(true)] " : string.Empty;
    public string TypeIsNotNull => type.IsReferenceType ? $" && {AsAlias} is not null" : string.Empty;
    public string MemberNotNullWhen => type.IsReferenceType ? $"[MemberNotNullWhen(true, \"{AsAlias}\")]" : string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static string GetTypeAlias(ITypeSymbol type) {
        switch (type) {
            case INamedTypeSymbol {IsTupleType: true } namedType: {
                string stringConcat = string.Join(
                    "And",
                    namedType.TupleElements.Select(e => GetTypeAlias(e.Type))
                );
                return $"{stringConcat}Tuple";
            }

            case INamedTypeSymbol {IsGenericType: true, TypeArguments.Length: > 0 } namedType: {
                string stringConcat = string.Join(
                    "And",
                    namedType.TypeArguments
                        .Where(arg => arg is not ITypeParameterSymbol)
                        .Select(GetTypeAlias)
                );
                
                return stringConcat.Length > 0 ?
                    $"{namedType.Name}Of{stringConcat}":
                    namedType.Name; // It might be that all type parameters are generic, but we don't want to show that in the alias
            }

            case INamedTypeSymbol namedType: {
                return namedType.Name;
            }

            case IArrayTypeSymbol arrayType: {
                return $"{GetTypeAlias(arrayType.ElementType)}Array";
            }
            
            default:
                return type.Name;
        }
    }
}
