// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Tests.AterraEngine.Unions.Generator;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class IncrementalGeneratorTest<TTest, TGenerator> where TGenerator : IIncrementalGenerator, new() {
    protected abstract Type[] ReferenceTypes { get; }

    protected void TestGenerator(string input, string expectedOutput, Func<GeneratedSourceResult, bool> predicate) {
        var generator = new TGenerator();
        var driver = CSharpGeneratorDriver.Create(generator.AsSourceGenerator());

        var compilation = CSharpCompilation.Create(
            typeof(TTest).Name,
            [CSharpSyntaxTree.ParseText(input)],
            ReferenceTypes.Select(t => MetadataReference.CreateFromFile(t.Assembly.Location)).ToArray(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        GeneratorDriverRunResult driverRunResult = driver.RunGenerators(compilation).GetRunResult();

        Assert.NotEmpty(driverRunResult.GeneratedTrees);
        foreach (Diagnostic diagnostic in driverRunResult.Diagnostics.Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)) {
            Debug.WriteLine($"Error Diagnostic: {diagnostic.GetMessage()}");
            Console.WriteLine($"Error Diagnostic: {diagnostic.GetMessage()}");
        }

        GeneratedSourceResult? generatedSource = driverRunResult.Results
            .SelectMany(result => result.GeneratedSources)
            .SingleOrDefault(predicate);

        Assert.NotNull(generatedSource?.SourceText);
        Assert.Equal(expectedOutput.Trim(), generatedSource.Value.SourceText.ToString().Trim(),
            ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
    }

}
