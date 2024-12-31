// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionAliasesAttributeTest {

    [Test]
    public async Task UnionAliasesAttribute_ShouldHaveCorrectAliases_WhenAliasT0IsProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute(aliasT0: "SuccessWithValue");

        // Assert
        await Assert.That(attribute.Aliases.Select(a => a ?? "")).Contains("SuccessWithValue");
    }


    [Test]
    public async Task UnionAliasesAttribute_ShouldHaveCorrectAliases_WhenMultipleAliasesAreProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute("Alias0", "Alias1", "Alias2");

        // Assert
        await Assert.That(attribute.Aliases[0]).IsTypeOf<string>().And.IsEqualTo("Alias0");
        await Assert.That(attribute.Aliases[1]).IsTypeOf<string>().And.IsEqualTo("Alias1");
        await Assert.That(attribute.Aliases[2]).IsTypeOf<string>().And.IsEqualTo("Alias2");
    }

    [Test]
    public async Task UnionAliasesAttribute_ShouldHaveCorrectAliases_WhenAliasT1IsProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute(aliasT1: "Empty");

        // Assert
        await Assert.That(attribute.Aliases[1]).IsTypeOf<string>().And.IsEqualTo("Empty");
    }


    [Test]
    public async Task UnionAliasesAttribute_ShouldHaveDefaultAliases_WhenNoAliasesAreProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute();

        // Assert
        foreach (string? alias in attribute.Aliases) {
            await Assert.That(alias).IsNullOrWhitespace();
        }
    }

    [Test]
    public async Task UnionAliasesAttribute_ShouldHaveCorrectAliases_WhenAliasT0AndAliasT1AreProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute(aliasT0: "Nothing", aliasT1: "Something");

        // Assert
        await Assert.That(attribute.Aliases[0]).IsTypeOf<string>().And.IsEqualTo("Nothing");
        await Assert.That(attribute.Aliases[1]).IsTypeOf<string>().And.IsEqualTo("Something");
    }


    [Test]
    public async Task UnionAliasesAttribute_ShouldHaveCorrectNumberOfAliases() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute("Alias0", "Alias1");

        // Assert
        await Assert.That(attribute.Aliases[0]).IsTypeOf<string>().And.IsEqualTo("Alias0");
        await Assert.That(attribute.Aliases[1]).IsTypeOf<string>().And.IsEqualTo("Alias1");
    }

    [Test]
    public async Task UnionAliasesAttribute_ShouldHaveCorrectAliases_WhenAliasT2IsProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute(aliasT2: "Alias");

        // Assert
        await Assert.That(attribute.Aliases[2]).IsTypeOf<string>().And.IsEqualTo("Alias");
    }


    [Test]
    public async Task UnionAliasesAttribute_ShouldHandleMixedValues() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute("Alias0", null, "Alias2");

        // Assert
        await Assert.That(attribute.Aliases[0]).IsTypeOf<string>().And.IsEqualTo("Alias0");
        await Assert.That(attribute.Aliases[1]).IsNull();
        await Assert.That(attribute.Aliases[2]).IsTypeOf<string>().And.IsEqualTo("Alias2");
    }
}
