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
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        // Detect types with the IUnion<> interface
        IncrementalValueProvider<ImmutableArray<UnionObject>> unionStructs = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (node, _) => node is StructDeclarationSyntax or RecordDeclarationSyntax {ClassOrStructKeyword.ValueText: "struct"},
                GatherUnionStructInfo)
            .Where(info => info is not null)
            .Select((info, _) => new UnionObject(
                info?.StructName!,
                info?.Namespace!,
                info?.TypesWithAliases!,
                (ImmutableArray<string>)info?.TypeParameters!,
                info is { IsRecordStruct: true }   
            ))
            .Collect();

        // Register the source output
        context.RegisterSourceOutput(context.CompilationProvider.Combine(unionStructs), GenerateSources);
    }

    private static UnionObject? GatherUnionStructInfo(GeneratorSyntaxContext context, CancellationToken cancellationToken) {
        var interfaces = ImmutableArray<INamedTypeSymbol>.Empty;
        var attributes = ImmutableArray<AttributeData>.Empty;
        string name = string.Empty;
        string nameSpace = string.Empty;
        var typeParams = ImmutableArray<ITypeParameterSymbol>.Empty;
        bool isRecordStruct = false;
        
        switch (context.Node) {
            case RecordDeclarationSyntax recordDeclarationSyntax : {
                if (context.SemanticModel.GetDeclaredSymbol(recordDeclarationSyntax) is not {} recordSymbol) return null;
                interfaces = recordSymbol.Interfaces;
                attributes = recordSymbol.GetAttributes();
                name = recordSymbol.Name;
                nameSpace = recordSymbol.ContainingNamespace.ToDisplayString();
                typeParams = recordSymbol.TypeParameters;
                isRecordStruct = true;
                break;
            }
            case StructDeclarationSyntax structDeclarationSyntax: {
                if (context.SemanticModel.GetDeclaredSymbol(structDeclarationSyntax) is not {} structSymbol) return null;
                interfaces = structSymbol.Interfaces;
                attributes = structSymbol.GetAttributes();
                name = structSymbol.Name;
                nameSpace = structSymbol.ContainingNamespace.ToDisplayString();
                typeParams = structSymbol.TypeParameters;
                break;
            }
        }
        // Check if the struct implements IUnion<>
        INamedTypeSymbol? iUnionInterface = interfaces.FirstOrDefault(i => i.Name.Equals("IUnion") && i.IsGenericType);
        if (iUnionInterface is null) return null;

        // Extract the type arguments from IUnion<>
        ImmutableArray<ITypeSymbol> typeArguments = [..iUnionInterface.TypeArguments];

        // Fetch aliases from the UnionAliases attribute
        AttributeData? aliasAttributeData = attributes
            .FirstOrDefault(attr => attr.AttributeClass?.Name == "UnionAliasesAttribute");

        Dictionary<ITypeSymbol, string?> typesWithAliases = ExtractTypesWithAliases(aliasAttributeData, typeArguments);

        return new UnionObject(
            name,
            nameSpace,
            typesWithAliases,
            [..typeParams.Select(tp => tp.ToDisplayString())],
            isRecordStruct
        );
    }

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

    private static void GenerateSources(SourceProductionContext context, (Compilation, ImmutableArray<UnionObject>) source) {
        (_, ImmutableArray<UnionObject> classDeclarations) = source;
        Dictionary<string, int> generatedUnions = [];
        foreach (UnionObject unionInfo in classDeclarations) {
            int i = 0;
            if (generatedUnions.TryGetValue(unionInfo.StructName, out int value)) i = value + 1;
            context.AddSource($"{unionInfo.Namespace}.{unionInfo.StructName}_{i}_Union.g.cs", GenerateUnionCode(unionInfo));
            generatedUnions[unionInfo.StructName] = i;
        }
    }

    private static string GenerateUnionCode(UnionObject unionObject) {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("// <auto-generated />");
        
        var namespaces = new HashSet<string> {
            "System",
            "System.Diagnostics.CodeAnalysis",
            "System.Threading.Tasks"
        };

        namespaces.UnionWith(unionObject.TypesWithAliases.Keys
            .Select(type => type.ContainingNamespace?.ToDisplayString())
            .Where(ns => !string.IsNullOrEmpty(ns) && ns != unionObject.Namespace)!);

        foreach (string? ns in namespaces) {
            stringBuilder.AppendLine($"using {ns};");
        }

        string isRecord = unionObject.IsRecordStruct ? "record " : string.Empty;
        stringBuilder
            .AppendLine($"namespace {unionObject.Namespace};")
            .AppendLine("#nullable enable")
            .AppendLine($"public readonly partial {isRecord}struct {unionObject.GetStructClassName()} {{")
            .AppendLine();

        Dictionary<ITypeSymbol, UnionStringValues> typeToStringValues = [];
        
        #region Per Type properties
        foreach (KeyValuePair<ITypeSymbol, string?> kvp in unionObject.TypesWithAliases) {
            UnionStringValues sv = new(kvp.Key, kvp.Value);
            typeToStringValues.Add(kvp.Key, sv);

            stringBuilder
                .AppendLine($"    #region {sv.Alias}")
                .AppendLine($"    {sv.MemberNotNullWhen}public bool {sv.IsAlias} {{ get; init; }} = false;")
                .AppendLine($"    public {sv.Type}{sv.TypeNullable} {sv.AsAlias} {{get; init;}} = default!;")
                .AppendLine($"    public bool TryGet{sv.AsAlias}({sv.NotNullWhen}out {sv.Type}{sv.TypeNullable} value) {{")
                .AppendLine($"        if ({sv.IsAlias}{sv.TypeIsNotNull}) {{")
                .AppendLine($"            value = {sv.AsAlias};")
                .AppendLine( "            return true;")
                .AppendLine( "        }")
                .AppendLine( "        value = default;")
                .AppendLine( "        return false;")
                .AppendLine( "    }")
                .AppendLine($"    public static implicit operator {unionObject.GetStructClassName()}({sv.Type} value) => new {unionObject.GetStructClassName()}() {{")
                .AppendLine($"        {sv.IsAlias} = true,")
                .AppendLine($"        {sv.AsAlias} = value")
                .AppendLine( "    };")
                .AppendLine( "    #endregion")
                .AppendLine();
        }
        #endregion

        #region Value Property
        stringBuilder.AppendLine( "    public object? Value { get {");
        foreach (UnionStringValues sv in typeToStringValues.Values) {
            stringBuilder.AppendLine($"        if ({sv.IsAlias}) return {sv.AsAlias};");
        }
        stringBuilder
            .AppendLine( """        throw new ArgumentException("Union does not contain a value");""")
            .AppendLine( "    }}")
            .AppendLine();
        #endregion
        
        #region Match Methods
        stringBuilder.AppendLine("    #region Match and MatchAsync");
        stringBuilder.Append( "    public TOutput Match<TOutput>(");
        stringBuilder.Append(string.Join(",", typeToStringValues.Values.Select(sv => $"Func<{sv.Type}, TOutput> {sv.Alias.ToLowerInvariant()}Case")));
        stringBuilder.AppendLine( "){");

        stringBuilder.AppendLine("        switch (this) {");
        foreach (UnionStringValues sv in typeToStringValues.Values) {
            stringBuilder.AppendLine($"            case {{{sv.IsAlias}: true, {sv.AsAlias}: var value}} : return {sv.Alias.ToLowerInvariant()}Case(value); ");

        }
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("        throw new ArgumentException(\"Union does not contain a value\");");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine();
        
        
        stringBuilder.Append( "    public async Task<TOutput> MatchAsync<TOutput>(");
        stringBuilder.Append(string.Join(",", typeToStringValues.Values.Select(sv => $"Func<{sv.Type}, Task<TOutput>> {sv.Alias.ToLowerInvariant()}Case")));
        stringBuilder.AppendLine( "){");

        stringBuilder.AppendLine("        switch (this) {");
        foreach (UnionStringValues sv in typeToStringValues.Values) {
            stringBuilder.AppendLine($"            case {{{sv.IsAlias}: true, {sv.AsAlias}: var value}} : return await {sv.Alias.ToLowerInvariant()}Case(value); ");
        }
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("        throw new ArgumentException(\"Union does not contain a value\");");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("    #endregion");
        stringBuilder.AppendLine();
        #endregion
        
        #region Switch Methods
        stringBuilder.AppendLine("    #region Switch and SwitchAsync");
        stringBuilder.Append( "    public void Switch(");
        stringBuilder.Append(string.Join(",", typeToStringValues.Values.Select(sv => $"Action<{sv.Type}> {sv.Alias.ToLowerInvariant()}Case")));
        stringBuilder.AppendLine( "){");

        stringBuilder.AppendLine("        switch (this) {");
        foreach (UnionStringValues sv in typeToStringValues.Values) {
            stringBuilder.AppendLine($"            case {{{sv.IsAlias}: true, {sv.AsAlias}: var value}} : {sv.Alias.ToLowerInvariant()}Case(value); return;");
        }
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("        throw new ArgumentException(\"Union does not contain a value\");");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine();
        
        
        stringBuilder.Append( "    public async Task SwitchAsync(");
        stringBuilder.Append(string.Join(",", typeToStringValues.Values.Select(sv => $"Func<{sv.Type}, Task> {sv.Alias.ToLowerInvariant()}Case")));
        stringBuilder.AppendLine( "){");

        stringBuilder.AppendLine("        switch (this) {");
        foreach (UnionStringValues sv in typeToStringValues.Values) {
            stringBuilder.AppendLine($"            case {{{sv.IsAlias}: true, {sv.AsAlias}: var value}} : await {sv.Alias.ToLowerInvariant()}Case(value); return;");
        }
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("        throw new ArgumentException(\"Union does not contain a value\");");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("    #endregion");
        stringBuilder.AppendLine();
        #endregion
        
        stringBuilder.AppendLine("}");
        

        return stringBuilder.ToString();
    }
}
