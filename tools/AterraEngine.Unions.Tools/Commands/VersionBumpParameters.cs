// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser;

namespace AterraEngine.Unions.Tools.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class VersionBumpParameters : ICommandParameters {
    [ArgValue("root")] [Description("The root directory of the project to update")]
    public string Root { get; set; } = "../../../../../";
    
    [ArgValue("section")] [Description("The section of the version to bump. One of: Major, Minor, Patch")]
    public string? SectionStringValue { get; set; }
    
    public VersionSection Section => Enum.Parse<VersionSection>(SectionStringValue ?? "None", ignoreCase:true);
}

public enum VersionSection {
    Major,
    Minor,
    Patch,
    None
}
