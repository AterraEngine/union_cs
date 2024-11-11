// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.AterraEngine.Unions.Generator;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class IncrementalGeneratorTest<TGenerator> where TGenerator : IIncrementalGenerator, new() {
    protected abstract Type[] ReferenceTypes { get; }

    protected async Task TestGeneratorAsync(string input, string expectedOutput, Func<GeneratedSourceResult, bool> predicate) {
        using var workspace = new AdhocWorkspace();
        
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TGenerator())
            .WithUpdatedParseOptions(new CSharpParseOptions(LanguageVersion.Latest));
        
        Project project = workspace.CurrentSolution
            .AddProject("TestProject", "TestProject.dll", "C#")
            .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithPlatform(Platform.AnyCpu)
            )
            .WithParseOptions(new CSharpParseOptions(LanguageVersion.Latest));

        project = project.AddDocument("Test.cs", input).Project;
        project = ReferenceTypes.Aggregate(
            project, 
            (current, type) => current.AddMetadataReference(MetadataReference.CreateFromFile(type.Assembly.Location))
        );

        Compilation? compilation = await project.GetCompilationAsync();
        Assert.NotNull(compilation);
        
        GeneratorDriverRunResult runResult = driver.RunGenerators(compilation).GetRunResult();
            
        Assert.NotEmpty(runResult.GeneratedTrees);
        foreach (Diagnostic diagnostic in runResult.Diagnostics.Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)) {
            Debug.WriteLine($"Error Diagnostic: {diagnostic.GetMessage()}");
            Console.WriteLine($"Error Diagnostic: {diagnostic.GetMessage()}");
        }

        GeneratedSourceResult? generatedSource = runResult.Results
            .SelectMany(result => result.GeneratedSources)
            .SingleOrDefault(predicate);

        Assert.NotNull(generatedSource?.SourceText);
        Assert.Equal(expectedOutput.Trim(), generatedSource.Value.SourceText.ToString().Trim(),
            ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
    }

}
