// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Tests.AterraEngine.Unions.Generator;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class IncrementalGeneratorTest<TGenerator> where TGenerator : IIncrementalGenerator, new() {
    protected abstract Assembly[] ReferenceAssemblies { get; }

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
        project = ReferenceAssemblies.Aggregate(
            project, 
            (current, assembly) => current.AddMetadataReference(MetadataReference.CreateFromFile(assembly.Location))
        );

        Compilation? compilation = await project.GetCompilationAsync();
        Assert.NotNull(compilation);
        
        GeneratorDriverRunResult runResult = driver.RunGenerators(compilation).GetRunResult();
            
        Assert.NotEmpty(runResult.GeneratedTrees);
        foreach (Diagnostic diagnostic in runResult.Diagnostics.Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)) {
            Console.WriteLine($"Error Diagnostic: {diagnostic.GetMessage()}");
        }

        foreach (Diagnostic diagnostic in compilation.GetDiagnostics()) {
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
