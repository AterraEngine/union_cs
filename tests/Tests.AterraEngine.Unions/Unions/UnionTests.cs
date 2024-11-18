// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionTests {
    
    [Fact]
    public void Test_UnionHasT0() {
        Union<string, int> union = "Anna";
        Assert.True(union.IsT0);
        Assert.False(union.IsT1);
        Assert.IsType<string>(union.Value);
        Assert.Equal("Anna", union.AsT0);
        Assert.True(union.TryGetAsT0(out string result));
        Assert.Equal("Anna", result);
        Assert.False(union.TryGetAsT1(out int _));
    }
}
