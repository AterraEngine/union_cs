// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;

namespace AterraEngine.Unions.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Rules {
    public static readonly DiagnosticDescriptor NoAttributesFound = new(
        id: "ATRUN001",
        title: "InjectableServiceAttribute not found",
        messageFormat: "InjectableServiceAttribute not found",
        category: "InjectableServices",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true
    );
}
