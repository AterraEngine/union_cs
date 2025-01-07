// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.GeneratorTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace AterraEngine.Unions.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Generates source code for union-like structures in C#.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class UnionGenerator : IIncrementalGenerator {
    /// <summary>
    ///     Initializes the incremental generator by registering syntax providers and source output generation.
    /// </summary>
    /// <param name="context">The context used to configure incremental source generation.</param>
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        // Detect types with the IUnion<> interface
        IncrementalValueProvider<ImmutableArray<UnionObject>> unionStructs = context.SyntaxProvider
            .CreateSyntaxProvider(
                IsUnionStructCandidate,
                GatherUnionStructInfo)
            .Collect();

        // Register the source output
        context.RegisterSourceOutput(context.CompilationProvider.Combine(unionStructs), GenerateSources);
    }

    /// <summary>
    ///     Identifies whether a given syntax node is a candidate for being a union struct.
    ///     A union struct candidate must be a struct or record struct that implements
    ///     an interface with a name containing "IUnion".
    /// </summary>
    /// <param name="node">The syntax node to evaluate.</param>
    /// <param name="_">An instance of <see cref="CancellationToken" /> to handle cancellation requests.</param>
    /// <returns>
    ///     A boolean value indicating whether the provided syntax node is a valid union struct candidate.
    /// </returns>
    private static bool IsUnionStructCandidate(SyntaxNode node, CancellationToken _) {
        if (node is
            not ((StructDeclarationSyntax or RecordDeclarationSyntax { ClassOrStructKeyword.ValueText: "struct" })
            and BaseTypeDeclarationSyntax { BaseList.Types.Count : > 0 } baseTypeDeclarationSyntax)) return false;

        return baseTypeDeclarationSyntax.BaseList.Types[0].Type switch {
            GenericNameSyntax { Identifier.ValueText: var valueText } => valueText.Contains("IUnion"),
            QualifiedNameSyntax qualifiedNameSyntax => qualifiedNameSyntax.ToFullString().Contains("IUnion"),
            _ => false
        };
    }

    /// <summary>
    ///     Gathers information about a struct or record struct that implements the IUnion interface.
    /// </summary>
    /// <param name="context">
    ///     Provides context for analyzing syntax elements and obtaining semantic information.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token that can be used to cancel the operation if needed.
    /// </param>
    /// <returns>
    ///     A <see cref="UnionObject" /> containing details about the struct or record struct,
    ///     such as its name, namespace, type parameters, aliases, and whether it is a record struct.
    /// </returns>
    private static UnionObject GatherUnionStructInfo(GeneratorSyntaxContext context, CancellationToken cancellationToken) {
        bool isRecordStruct = context.Node switch {
            RecordDeclarationSyntax => true,
            _ => false
        };

        INamedTypeSymbol namedTypeSymbol = context.Node switch {
            RecordDeclarationSyntax recordDeclarationSyntax => context.SemanticModel.GetDeclaredSymbol(recordDeclarationSyntax)!,
            StructDeclarationSyntax structDeclarationSyntax => context.SemanticModel.GetDeclaredSymbol(structDeclarationSyntax)!,
            _ => throw new ArgumentOutOfRangeException()// Should never happen because we check in IsUnionStructCandidate
        };

        // Check if the struct implements IUnion<>
        INamedTypeSymbol iUnionInterface = namedTypeSymbol.Interfaces.First(
            i => i.Name.Equals("IUnion") && i.IsGenericType
        );

        // Fetch aliases from the UnionAliases attribute
        AttributeData? aliasAttributeData = namedTypeSymbol.GetAttributes()
            .FirstOrDefault(attr => attr.AttributeClass?.Name == "UnionAliasesAttribute");
        
        AttributeData? extraAttributeData = namedTypeSymbol.GetAttributes()
            .FirstOrDefault(attr => attr.AttributeClass?.Name == "UnionExtraAttribute");

        Dictionary<ITypeSymbol, string?> typesWithAliases = ExtractTypesWithAliases(
            aliasAttributeData,
            iUnionInterface.TypeArguments
        );

        return new UnionObject(
            namedTypeSymbol.Name,
            namedTypeSymbol.ContainingNamespace.ToDisplayString(),
            typesWithAliases,
            [..namedTypeSymbol.TypeParameters.Select(tp => tp.ToDisplayString())],
            isRecordStruct,
            extraAttributeData?.ConstructorArguments.FirstOrDefault().Value as int? ?? 0
        );
    }

    /// <summary>
    ///     Extracts a dictionary of type symbols and their corresponding alias strings (if any)
    ///     from the provided attribute data and collection of type arguments.
    /// </summary>
    /// <param name="aliasAttributeData">
    ///     The <see cref="AttributeData" /> object representing the UnionAliases attribute that contains alias information.
    ///     This can be null if no alias attribute is specified.
    /// </param>
    /// <param name="typeArguments">
    ///     An immutable array of type symbols representing the type arguments for a union type.
    /// </param>
    /// <returns>
    ///     A dictionary where each key is a type symbol representing a type argument, and the value is its corresponding alias
    ///     string
    ///     (if provided in the UnionAliases attribute). If no alias is provided, the value will be null.
    /// </returns>
    private static Dictionary<ITypeSymbol, string?> ExtractTypesWithAliases(AttributeData? aliasAttributeData, ImmutableArray<ITypeSymbol> typeArguments) {
        int maxLength = typeArguments.Length;
        string?[] aliases = new string?[maxLength];

        // ReSharper disable once InvertIf
        if (aliasAttributeData is { ConstructorArguments : { Length: > 0 } arguments }) {
            for (int i = 0; i < maxLength; i++) {
                aliases[i] = arguments[i].Value as string;
            }
        }

        return typeArguments.Zip(aliases, resultSelector: (type, alias) => (type, alias))
            .ToDictionary<(ITypeSymbol type, string? alias), ITypeSymbol, string?>(
                keySelector: tuple => tuple.type,
                elementSelector: tuple => tuple.alias,
                SymbolEqualityComparer.Default
            );
    }

    /// <summary>
    ///     Generates source code for union structures based on the provided compilation and collected union object data.
    /// </summary>
    /// <param name="context">The source production context used to add generated source files.</param>
    /// <param name="source">
    ///     A tuple consisting of the current compilation and an immutable array of union objects gathered
    ///     from the codebase.
    /// </param>
    private static void GenerateSources(SourceProductionContext context, (Compilation, ImmutableArray<UnionObject>) source) {
        (_, ImmutableArray<UnionObject> classDeclarations) = source;
        Dictionary<string, int> generatedUnions = [];
        foreach (UnionObject unionInfo in classDeclarations) {
            int i = 0;
            if (generatedUnions.TryGetValue(unionInfo.StructName, out int value)) i = value + 1;
            string iOffset = i > 0 ? $"_{i}" : string.Empty;
            context.AddSource($"{unionInfo.StructName}{iOffset}_Union.g.cs", GenerateUnionCode(unionInfo));
            generatedUnions[unionInfo.StructName] = i;
        }
    }

    /// <summary>
    ///     Generates C# union code for the specified union object.
    /// </summary>
    /// <param name="unionObject">
    ///     The union object containing metadata and types needed to generate the union code.
    /// </param>
    /// <returns>
    ///     A string representing the generated C# code for the union.
    /// </returns>
    private static string GenerateUnionCode(UnionObject unionObject) {
        var builder = new GeneratorStringBuilder();
        string isRecord = unionObject.IsRecordStruct ? "record " : string.Empty;

        builder
            .AppendAutoGenerated()
            .AppendMultipleUsings(
                // Default services which MUST be included
                () => [
                    "System",
                    "System.Diagnostics.CodeAnalysis",
                    "System.Threading.Tasks"
                ],
                // I don't remember why, but this is how it was in the old system
                () => unionObject.TypesWithAliases.Keys
                    .Select(type => type.ContainingNamespace?.ToDisplayString())
                    .Where(ns => !string.IsNullOrEmpty(ns) && ns != unionObject.Namespace)!
            )
            .AppendNamespace(unionObject.Namespace)
            .AppendNullableEnable()
            .AppendLine($"public readonly partial {isRecord}struct {unionObject.GetStructClassName()} {{")
            .AppendLine();

        Dictionary<ITypeSymbol, UnionStringValues> typeToStringValues = [];

        #region Per Type properties
        foreach (KeyValuePair<ITypeSymbol, string?> kvp in unionObject.TypesWithAliases) {
            UnionStringValues sv = new(kvp.Key, kvp.Value);
            typeToStringValues.Add(kvp.Key, sv);

            builder
                .AppendLineIndented($"#region {sv.Alias}")
                .Indent(g => g
                    .AppendLine($"{sv.MemberNotNullWhen}public bool {sv.IsAlias} {{ get; private init; }} = false;")
                    .AppendLine($"{sv.MemberNotNullWhen}public {sv.Type}{sv.TypeNullable} {sv.AsAlias} {{get; private init;}} = default!;").Indent(g2 => g2
                        .AppendLine($"if ({sv.IsAlias}{sv.TypeIsNotNull}) {{").Indent(g3 => g3
                            .AppendLine($"value = {sv.AsAlias};")
                            .AppendLine("return true;")
                        )
                        .AppendLine("}")
                        .AppendLine("value = default;")
                        .AppendLine("return false;")
                    )
                    .AppendLine("}")
                    .AppendLine($"public static implicit operator {unionObject.GetStructClassName()}({sv.Type} value) => new {unionObject.GetStructClassName()}() {{").Indent(g2 => g2
                        .AppendLine($"{sv.IsAlias} = true,")
                        .AppendLine($"{sv.AsAlias} = value")
                    )
                    .AppendLine("};")
                );

            if (unionObject.HasFlagGenerateFrom()) {
                builder.Indent(g => g
                    .AppendLine($"public static {unionObject.GetStructClassName()} From{sv.Alias}({sv.Type} value) => new {unionObject.GetStructClassName()}() {{").Indent(g2 => g2
                        .AppendLine($"{sv.IsAlias} = true,")
                        .AppendLine($"{sv.AsAlias} = value")
                    )
                    .AppendLine("};")
                );
            }
            
            if (unionObject.HasFlagGenerateAsValue() && unionObject.IsValidGenerateAsValue(sv.TypeSymbol, out bool isValues, out string valueTypeName, out string notNullWhen, out string nullable)) {
                string s = isValues ? "s" : string.Empty;
                builder.Indent(g => g
                    .AppendLine($"public bool TryGet{sv.AsAlias}Value{s}({notNullWhen}out {valueTypeName}{nullable} value{s}) {{").Indent(g1 => g1
                        .AppendLine($"if ({sv.IsAlias}{sv.TypeIsNotNull}) {{").Indent(g2 => g2
                            .AppendLine($"value{s} = {sv.AsAlias}.Value{s};")
                            .AppendLine("return true;")
                        )
                        .AppendLine("}")
                        .AppendLine($"value{s} = default;")
                        .AppendLine("return false;")
                    )
                );
            }
            
            builder.AppendLineIndented("#endregion");
        }
        #endregion

        #region Value Property
        builder.Indent(g => {
            g.AppendLine("public object? Value { get {");
            foreach (UnionStringValues sv in typeToStringValues.Values) {
                g.Indent(g1 => g1
                    .AppendLine($"if ({sv.IsAlias}) return {sv.AsAlias};")
                );
            }
            g.Indent(g1 => g1.AppendLine("""throw new ArgumentException("Union does not contain a value");"""));
            g.AppendLine("}}");
            g.AppendLine();
        });
        #endregion

        #region Match Methods
        string matchArgs = string.Join(",", typeToStringValues.Values.Select(sv => $"Func<{sv.Type}, TOutput> {sv.Alias.ToLowerInvariant()}Case"));
        string matchArgsAsync = string.Join(",", typeToStringValues.Values.Select(sv => $"Func<{sv.Type}, Task<TOutput>> {sv.Alias.ToLowerInvariant()}Case"));

        builder
            .AppendLineIndented("#region Match and MatchAsync")
            .Indent(g => g
                .AppendLine($"public TOutput Match<TOutput>({matchArgs}){{").Indent(g1 => {
                    g1.AppendLine("switch (this) {");
                    foreach (UnionStringValues sv in typeToStringValues.Values) {
                        g1.AppendLineIndented($"case {{{sv.IsAlias}: true, {sv.AsAlias}: var value}} : return {sv.Alias.ToLowerInvariant()}Case(value);");
                    }

                    g1.AppendLine("}");
                    g1.AppendLine("throw new ArgumentException(\"Union does not contain a value\");");
                })
                .AppendLine("}")
            )
            .AppendLine()
            .Indent(g => g
                .AppendLine($"public async Task<TOutput> MatchAsync<TOutput>({matchArgsAsync}){{").Indent(g1 => {
                    g1.AppendLine("switch (this) {");
                    foreach (UnionStringValues sv in typeToStringValues.Values) {
                        g1.AppendLineIndented($"case {{{sv.IsAlias}: true, {sv.AsAlias}: var value}} : return await {sv.Alias.ToLowerInvariant()}Case(value);");
                    }
                    g1.AppendLine("}");
                    g1.AppendLine("throw new ArgumentException(\"Union does not contain a value\");");
                })
                .AppendLine("}")
            )
            .AppendLineIndented("#endregion")
            .AppendLine();
        #endregion

        #region Switch Methods
        string switchArgs = string.Join(",", typeToStringValues.Values.Select(sv => $"Action<{sv.Type}> {sv.Alias.ToLowerInvariant()}Case"));
        string switchArgsAsync = string.Join(",", typeToStringValues.Values.Select(sv => $"Func<{sv.Type}, Task> {sv.Alias.ToLowerInvariant()}Case"));
        
        builder
            .AppendLineIndented("#region Switch and SwitchAsync")
            .Indent(g => g
                .AppendLine($"public void Switch({switchArgs}){{").Indent(g1 => {
                    g1.AppendLine("switch (this) {");
                    foreach (UnionStringValues sv in typeToStringValues.Values) {
                        g1.AppendLineIndented($"case {{{sv.IsAlias}: true, {sv.AsAlias}: var value}} : {sv.Alias.ToLowerInvariant()}Case(value); return;");
                    }
                    g1.AppendLine("}");
                    g1.AppendLine("throw new ArgumentException(\"Union does not contain a value\");");
                })
                .AppendLine("}")
            )
            .AppendLine()
            .Indent(g => g
                .AppendLine($"public async Task SwitchAsync({switchArgsAsync}){{").Indent(g1 => {
                    g1.AppendLine("switch (this) {");
                    foreach (UnionStringValues sv in typeToStringValues.Values) {
                        g1.AppendLineIndented($"case {{{sv.IsAlias}: true, {sv.AsAlias}: var value}} : await {sv.Alias.ToLowerInvariant()}Case(value); return;");
                    }
                    g1.AppendLine("}");
                    g1.AppendLine("throw new ArgumentException(\"Union does not contain a value\");");
                })
                .AppendLine("}")
            )
            .AppendLineIndented("#endregion")
            .AppendLine();
        #endregion

        builder.AppendLine("}");
        return builder.ToString();
    }
}
