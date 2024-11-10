// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ManyNoneOrErrorTests {
    [Theory]
    [InlineData(new[] { 1, 2, 3 })]
    public void ImplicitConversion_FromMany_SetsIsManyTrue(int[] values) {
        // Arrange
        var many = new Many<int>(values);

        // Act
        ManyNoneOrError<int, string> union = many;

        // Assert
        Assert.True(union.IsMany);
        Assert.False(union.IsNone);
        Assert.False(union.IsError);

        Assert.Equal(many, union.AsMany);
    }

    [Fact]
    public void ImplicitConversion_FromNone_SetsIsNoneTrue() {
        // Arrange
        var none = new None();

        // Act
        ManyNoneOrError<int, string> union = none;

        // Assert
        Assert.True(union.IsNone);
        Assert.False(union.IsMany);
        Assert.False(union.IsError);

        Assert.Equal(none, union.AsNone);
    }

    [Theory]
    [InlineData("An error occurred")]
    public void ImplicitConversion_FromError_SetsIsErrorTrue(string errorMessage) {
        // Arrange
        var error = new Error<string>(errorMessage);

        // Act
        ManyNoneOrError<int, string> union = error;

        // Assert
        Assert.True(union.IsError);
        Assert.False(union.IsMany);
        Assert.False(union.IsNone);

        Assert.Equal(error, union.AsError);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 })]
    public void TryGetAsMany_WhenIsMany_ReturnsTrueAndSetsValue(int[] values) {
        // Arrange
        var many = new Many<int>(values);
        ManyNoneOrError<int, string> union = many;

        // Act
        bool success = union.TryGetAsMany(out Many<int> result);

        // Assert
        Assert.True(success);
        Assert.Equal(many, result);
    }

    [Fact]
    public void TryGetAsNone_WhenIsNone_ReturnsTrueAndSetsValue() {
        // Arrange
        var none = new None();
        ManyNoneOrError<int, string> union = none;

        // Act
        bool success = union.TryGetAsNone(out None result);

        // Assert
        Assert.True(success);
        Assert.Equal(none, result);
    }

    [Theory]
    [InlineData("An error occurred")]
    [InlineData("")]
    public void TryGetAsError_WhenIsError_ReturnsTrueAndSetsValue(string errorMessage) {
        // Arrange
        var error = new Error<string>(errorMessage);
        ManyNoneOrError<int, string> union = error;

        // Act
        bool success = union.TryGetAsError(out Error<string> result);

        // Assert
        Assert.True(success);
        Assert.Equal(error, result);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 })]
    public void TryGetAsError_WhenNotError_ReturnsFalseAndDefaultValue(int[] values) {
        // Arrange
        ManyNoneOrError<int, string> union = new Many<int>(values);

        // Act
        bool success = union.TryGetAsError(out Error<string> _);

        // Assert
        Assert.False(success);
    }
}
