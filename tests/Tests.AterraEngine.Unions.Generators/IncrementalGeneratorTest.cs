// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Tests.AterraEngine.Unions.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class IncrementalGeneratorTest<TGenerator> where TGenerator : IIncrementalGenerator, new() {
    protected abstract Assembly[] ReferenceAssemblies { get; }

    protected async Task<GeneratorDriverRunResult> RunGeneratorAsync(string input) {
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
            func: (current, assembly) => current.AddMetadataReference(MetadataReference.CreateFromFile(assembly.Location))
        );

        Compilation? compilation = await project.GetCompilationAsync();
        await Assert.That(compilation).IsNotNull();

        // The reason why we disable diagnostics is that we want to test the generator itself, not the compiler.
        //      The compiler throws an error because the user create code depends on the generated code to work properly by the compiler.
        // ImmutableArray<Diagnostic> diagnostics = compilation.GetDiagnostics();
        // Assert.Empty(diagnostics); 

        GeneratorDriverRunResult runResult = driver.RunGenerators(compilation!).GetRunResult();

        await Assert.That(runResult.GeneratedTrees.Length).IsNotEqualTo(0);
        foreach (Diagnostic diagnostic in runResult.Diagnostics.Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)) {
            Console.WriteLine($"Error Diagnostic: {diagnostic.GetMessage()}");
        }

        return runResult;
    }
}
