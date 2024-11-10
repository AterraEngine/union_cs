// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace AterraEngine.Unions.Generator;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Generator(LanguageNames.CSharp)]
public class UnionGenerator : IIncrementalGenerator {
    private const string UnionGeneratorAttributeName = "AterraEngine.Unions.UnionGeneratorAttribute";
    private const string UnionGeneratorAttributeNameShort = "UnionGeneratorAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        // Detect types with the UnionGenerator attribute
        IncrementalValueProvider<ImmutableArray<UnionObject>> unionStructs = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (node, _) => node is StructDeclarationSyntax { AttributeLists.Count: > 0 },
                GetUnionStructInfo)
            .Where(info => info is not null)
            .Collect()!;

        // Register the source output
        context.RegisterSourceOutput(context.CompilationProvider.Combine(unionStructs), GenerateSources);
    }

    private static UnionObject? GetUnionStructInfo(GeneratorSyntaxContext context, CancellationToken cancellationToken) {
        if (context.Node is not StructDeclarationSyntax structDeclaration) return null;
        if (context.SemanticModel.GetDeclaredSymbol(structDeclaration) is not {} structSymbol) return null;

        IEnumerable<AttributeSyntax> attributes = structDeclaration.AttributeLists.SelectMany(al => al.Attributes);
        AttributeSyntax? unionAttribute = attributes.FirstOrDefault(attr => {
            SymbolInfo symbolInfo = context.SemanticModel.GetSymbolInfo(attr);
            ISymbol? symbol = symbolInfo.Symbol ?? symbolInfo.CandidateSymbols.FirstOrDefault();

            if (symbol?.ContainingType?.ToDisplayString() is not {} containingType) return false;

            return containingType.StartsWith(UnionGeneratorAttributeName)
                || containingType.StartsWith(UnionGeneratorAttributeNameShort);
        });
        if (unionAttribute is null) return null;

        // Extract the attribute data
        AttributeData? attributeData = structSymbol.GetAttributes()
            .FirstOrDefault(attr => attr.AttributeClass?.ToDisplayString().StartsWith(UnionGeneratorAttributeName) == true);
        if (attributeData is null) return null;

        Dictionary<ITypeSymbol, string?> typesWithAliases = ExtractTypesWithAliases(unionAttribute, attributeData);
        return new UnionObject(structSymbol.Name, structSymbol.ContainingNamespace.ToDisplayString(), typesWithAliases);
    }

    private static Dictionary<ITypeSymbol, string?> ExtractTypesWithAliases(AttributeSyntax attributeSyntax, AttributeData attributeData) {
        ImmutableArray<ITypeSymbol> types = attributeData.AttributeClass?.TypeArguments ?? ImmutableArray<ITypeSymbol>.Empty;
        if (attributeSyntax.ArgumentList is null)
            return types.ToDictionary<ITypeSymbol, ITypeSymbol, string?>(keySelector: t => t, elementSelector: _ => null, SymbolEqualityComparer.Default);

        // Inits with nulls
        var aliases = new List<string?>(new string?[types.Length]);

        // Extract aliases based on "aliasT" prefix
        foreach (AttributeArgumentSyntax argument in attributeSyntax.ArgumentList.Arguments) {
            if (argument.NameColon?.Expression is not IdentifierNameSyntax identifierName) continue;

            string alias = identifierName.Identifier.ValueText;
            if (!alias.StartsWith("aliasT")) continue;

            int index = int.Parse(alias.Substring(6));
            if (argument.Expression is not LiteralExpressionSyntax literal) continue;

            aliases[index] = literal.Token.ValueText;
        }

        // Create a dictionary of type symbols with corresponding aliases, or default to null
        return types.Zip(aliases, resultSelector: (type, alias) => (type, alias))
            .ToDictionary<(ITypeSymbol type, string? alias), ITypeSymbol, string?>(keySelector: t => t.type, elementSelector: t => t.alias, SymbolEqualityComparer.Default);
    }


    private static void GenerateSources(SourceProductionContext context, (Compilation, ImmutableArray<UnionObject>) source) {
        Compilation? compilation = source.Item1;
        ImmutableArray<UnionObject> classDeclarations = source.Item2;

        if (compilation.GetTypeByMetadataName(UnionGeneratorAttributeName) is null) {
            context.ReportDiagnostic(Diagnostic.Create(Rules.NoAttributesFound, Location.None));
            return;
        }

        foreach (UnionObject? unionInfo in classDeclarations) {
            context.AddSource($"{unionInfo.Namespace}.{unionInfo.StructName}_Union.g.cs", GenerateUnionCode(unionInfo));
        }
    }

    private static string GenerateUnionCode(UnionObject unionObject) {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("// <auto-generated />");
        // Collect required namespaces
        var namespaces = new HashSet<string> { "System" };
        namespaces.UnionWith(unionObject.TypesWithAliases.Keys
            .Select(type => type.ContainingNamespace.ToDisplayString())
            .Where(ns => !string.IsNullOrEmpty(ns)));

        // Generate using directives for the namespaces
        foreach (string? ns in namespaces) {
            stringBuilder.AppendLine($"using {ns};");
        }

        stringBuilder.AppendLine($"namespace {unionObject.Namespace};");
        stringBuilder.AppendLine($"public readonly partial struct {unionObject.StructName} {{");
        stringBuilder.AppendLine("    public object Value { get; init; } = default!;");

        foreach (KeyValuePair<ITypeSymbol, string?> kvp in unionObject.TypesWithAliases) {
            ITypeSymbol? typeSymbol = kvp.Key;
            string alias = kvp.Value ?? GetAlias(kvp);
            string isAlias = $"Is{alias}";
            stringBuilder.AppendLine($"    public bool {isAlias} {{ get; init; }} = false;");
            stringBuilder.AppendLine($"    public {typeSymbol} As{alias} => ({typeSymbol})Value;");
            stringBuilder.AppendLine($"    public bool TryGetAs{alias}(out {typeSymbol} value) {{");
            stringBuilder.AppendLine($"        if ({isAlias}) {{");
            stringBuilder.AppendLine($"            value = As{alias};");
            stringBuilder.AppendLine("            return true;");
            stringBuilder.AppendLine("        }");
            stringBuilder.AppendLine("        value = default;");
            stringBuilder.AppendLine("        return false;");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine($"    public static implicit operator {unionObject.StructName}({typeSymbol} value) => new {unionObject.StructName}() {{");
            stringBuilder.AppendLine("        Value = value,");
            stringBuilder.AppendLine($"        {isAlias} = true");
            stringBuilder.AppendLine("    };");
        }

        stringBuilder.AppendLine("}");
        return stringBuilder.ToString();
    }

    private static string GetAlias(KeyValuePair<ITypeSymbol, string?> keyValuePair) => keyValuePair.Value ?? GetTypeAlias(keyValuePair.Key, true);

    private static string GetTypeAlias(ITypeSymbol type, bool skipGenerics = false) {
        if (type is not INamedTypeSymbol namedType) return type.Name;

        if (namedType.IsTupleType) {
            return "TupleOf" + string.Join("And", namedType.TupleElements.Select(e => GetTypeAlias(e.Type, skipGenerics)));
        }

        string name = namedType.Name;
        if (namedType.IsGenericType && !skipGenerics) {
            name += $"Of{string.Join("And", namedType.TypeArguments.Select(ta => GetTypeAlias(ta, skipGenerics)))}>";
        }

        return name;
    }
}
