// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionAliasesAttributeTest {

    [Fact]
    public void UnionAliasesAttribute_ShouldHaveCorrectAliases_WhenAliasT0IsProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute(aliasT0: "SuccessWithValue");

        // Assert
        Assert.Contains("SuccessWithValue", attribute.Aliases);
    }


    [Fact]
    public void UnionAliasesAttribute_ShouldHaveCorrectAliases_WhenMultipleAliasesAreProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute("Alias0", "Alias1", "Alias2");

        // Assert
        Assert.Equal("Alias0", attribute.Aliases[0]);
        Assert.Equal("Alias1", attribute.Aliases[1]);
        Assert.Equal("Alias2", attribute.Aliases[2]);
    }

    [Fact]
    public void UnionAliasesAttribute_ShouldHaveCorrectAliases_WhenAliasT1IsProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute(aliasT1: "Empty");

        // Assert
        Assert.Equal("Empty", attribute.Aliases[1]);
    }


    [Fact]
    public void UnionAliasesAttribute_ShouldHaveDefaultAliases_WhenNoAliasesAreProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute();

        // Assert
        Assert.All(attribute.Aliases, Assert.Null);
    }

    [Fact]
    public void UnionAliasesAttribute_ShouldHaveCorrectAliases_WhenAliasT0AndAliasT1AreProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute(aliasT0: "Nothing", aliasT1: "Something");

        // Assert
        Assert.Equal("Nothing", attribute.Aliases[0]);
        Assert.Equal("Something", attribute.Aliases[1]);
    }


    [Fact]
    public void UnionAliasesAttribute_ShouldHaveCorrectNumberOfAliases() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute("Alias0", "Alias1");

        // Assert
        Assert.Equal("Alias0", attribute.Aliases[0]);
        Assert.Equal("Alias1", attribute.Aliases[1]);
    }

    [Fact]
    public void UnionAliasesAttribute_ShouldHaveCorrectAliases_WhenAliasT2IsProvided() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute(aliasT2: "Alias");

        // Assert
        Assert.Equal("Alias", attribute.Aliases[2]);
    }


    [Fact]
    public void UnionAliasesAttribute_ShouldHandleMixedValues() {
        // Arrange & Act
        var attribute = new UnionAliasesAttribute("Alias0", null, "Alias2");

        // Assert
        Assert.Equal("Alias0", attribute.Aliases[0]);
        Assert.Null(attribute.Aliases[1]);
        Assert.Equal("Alias2", attribute.Aliases[2]);
    }
}
