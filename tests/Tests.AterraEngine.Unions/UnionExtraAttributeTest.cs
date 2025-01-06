// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionExtraAttributeTest {

    [Test]
    [Arguments(UnionExtra.GenerateFrom)]
    public async Task UnionExtraAttribute_ShouldHaveCorrectEnum(UnionExtra extra) {
        // Arrange & Act
        var attribute = new UnionExtraAttribute(extra);

        // Assert
        await Assert.That(attribute.Extra).IsEqualTo(extra);
    }
}
