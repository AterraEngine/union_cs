// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using System.Linq;

namespace AterraEngine.Unions.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a utility struct for evaluating string values and generating type-related metadata for a given type symbol.
/// </summary>
/// <remarks>
/// This struct is designed for internal use within the AterraEngine.Unions namespace and provides several properties
/// for determining type-specific information based on the provided type symbol and optional alias.
/// </remarks>
/// <param name="type">
/// The type symbol representing the primary type associated with this struct.
/// </param>
/// <param name="alias">
/// An optional alias for the given type, used to generate additional type-related metadata. If not provided,
/// an alias will be automatically generated based on the type symbol.
/// </param>
public readonly struct UnionStringValues(ITypeSymbol type, string? alias) {
    /// <summary>
    /// Represents the string type of union, derived from a symbol representation of a type.
    /// Provides the string form of the type name for usage in union generation.
    /// </summary>
    public readonly string Type = type.ToString();

    /// <summary>
    /// Represents a string alias for a given type within the union type generation logic.
    /// Provides a derived alias name for a type or uses the explicitly provided alias.
    /// </summary>
    public readonly string Alias = alias ?? GetTypeAlias(type);

    /// <summary>
    /// Represents a computed property that provides a formatted string
    /// indicating an alias status based on the associated type alias.
    /// </summary>
    public string IsAlias => $"Is{Alias}";
    
    /// <summary>
    /// Gets a string representation of the current union type value in the format "As{Alias}".
    /// The property is derived from the alias of the union type and serves as an accessor identifier
    /// in generated union type classes.
    /// </summary>
    public string AsAlias => $"As{Alias}";
    
    /// <summary>
    /// Represents a property that determines whether a nullable postfix ("?") should be applied to the type string
    /// based on whether the associated type is a reference type.
    /// </summary>
    public string TypeNullable => type.IsReferenceType ? "?" : string.Empty;
    
    /// <summary>
    /// A string property representing a nullable annotation in the form of `[NotNullWhen(true)]`
    /// appended conditionally based on the reference type status of the associated type.
    /// Indicates that the output value will not be null when the corresponding condition evaluates to true.
    /// </summary>
    public string NotNullWhen => type.IsReferenceType ? "[NotNullWhen(true)] " : string.Empty;
    
    /// <summary>
    /// Gets a string that represents a condition to verify if the associated type is not null.
    /// </summary>
    public string TypeIsNotNull => type.IsReferenceType ? $" && {AsAlias} is not null" : string.Empty;
    
    /// <summary>
    /// Indicates that the specified member will not be null when the containing method returns a specified value.
    /// </summary>
    public string MemberNotNullWhen => type.IsReferenceType ? $"[MemberNotNullWhen(true, \"{AsAlias}\")]" : string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Generates an alias string representation for the provided <see cref="ITypeSymbol"/>.
    /// </summary>
    /// <param name="type">The type symbol for which the alias should be generated.</param>
    /// <returns>A string representing an alias for the type.</returns>
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
