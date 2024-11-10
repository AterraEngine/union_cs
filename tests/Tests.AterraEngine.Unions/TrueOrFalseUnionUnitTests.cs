// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TrueOrFalseUnionUnitTests {
    [Fact]
    public void Test_UnionHasTrue() {
        TrueOrFalse union = new True();
        Assert.True(union.IsTrue);
        Assert.False(union.IsFalse);
        Assert.IsType<True>(union.Value);
    }

    [Fact]
    public void Test_UnionHasFalse() {
        TrueOrFalse union = new False();
        Assert.True(union.IsFalse);
        Assert.False(union.IsTrue);
        Assert.IsType<False>(union.Value);
    }

    [Fact]
    public void Test_TryGetAsTrue_Success() {
        TrueOrFalse union = new True();
        Assert.True(union.TryGetAsTrue(out True result));
        Assert.Equal(new True(), result);
    }

    [Fact]
    public void Test_TryGetAsFalse_Success() {
        TrueOrFalse union = new False();
        Assert.True(union.TryGetAsFalse(out False result));
        Assert.Equal(new False(), result);
    }

    [Fact]
    public void Test_TryGetAsTrue_Failure() {
        TrueOrFalse union = new False();
        Assert.False(union.TryGetAsTrue(out True result));
        Assert.Equal(default, result);
    }

    [Fact]
    public void Test_TryGetAsFalse_Failure() {
        TrueOrFalse union = new True();
        Assert.False(union.TryGetAsFalse(out False result));
        Assert.Equal(default, result);
    }

    [Fact]
    public void Test_ImplicitConversion() {
        var value = new True();
        TrueOrFalse union = value;
        Assert.True(union.IsTrue);
        Assert.False(union.IsFalse);
        Assert.IsType<True>(union.Value);
        Assert.Equal(value, union.AsTrue);
    }
}