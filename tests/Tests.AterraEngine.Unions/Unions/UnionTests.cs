// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionTests {

    [Test]
    public async Task Test_UnionHasT0() {
        Union<string, int> union = "Anna";

        await Assert.That(union.IsT0).IsTrue();
        await Assert.That(union.IsT1).IsFalse();
        await Assert.That(union.Value).IsTypeOf<string>();
        await Assert.That(union.Value).IsNotTypeOf<int>();
        await Assert.That(union.AsT0).IsEqualTo("Anna");
        await Assert.That(union.TryGetAsT0(out string? result)).IsTrue();
        await Assert.That(result).IsEqualTo("Anna");
        await Assert.That(union.TryGetAsT1(out int _)).IsFalse();
    }
}
