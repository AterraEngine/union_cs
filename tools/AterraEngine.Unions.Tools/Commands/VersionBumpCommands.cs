// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser;
using System.Xml.Linq;

namespace AterraEngine.Unions.Tools.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class VersionBumpCommands : ICommandAtlas {

    [Command<VersionBumpParameters>("bump")]
    public async Task VersionBumpCommand(VersionBumpParameters args) {
        string[] projectFiles = ["AterraEngine.Unions.csproj", "AterraEngine.Unions.Generators.csproj"];
        VersionSection sectionToBump = args.Section;

        foreach (string projectFile in projectFiles)
        {
            string path = Path.Combine(args.Root, projectFile);
            if (!File.Exists(path)) {
                Console.WriteLine($"Could not find {path}");
                continue;
            }

            XDocument document;
            await using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)) {
                document = await XDocument.LoadAsync(stream, LoadOptions.None, default);
            }
            
            XElement? versionElement = document
                .Descendants("PropertyGroup")
                .Elements("Version")
                .FirstOrDefault();

            if (versionElement == null) {
                Console.WriteLine($"Could not find version element in {path}");
                continue;
            }

            string[] versionParts = versionElement.Value.Split('.');
            if (versionParts.Length != 3) {
                Console.WriteLine($"Invalid version format: {versionElement.Value}");
                continue;
            }

            switch (sectionToBump) {
                case VersionSection.Major:
                    versionParts[0] = (int.Parse(versionParts[0]) + 1).ToString();
                    versionParts[1] = "0";
                    versionParts[2] = "0";
                    break;
                case VersionSection.Minor:
                    versionParts[1] = (int.Parse(versionParts[1]) + 1).ToString();
                    versionParts[2] = "0";
                    break;
                case VersionSection.Patch:
                    versionParts[2] = (int.Parse(versionParts[2]) + 1).ToString();
                    break;

                case VersionSection.None:
                default: {
                    Console.WriteLine($"Invalid version section: {sectionToBump}");
                    return;
                }
            }

            versionElement.Value = string.Join(".", versionParts);

            await using (var stream = new FileStream(projectFile, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true)) {
                await document.SaveAsync(stream, SaveOptions.None, default);
            }
        }
    }
}
