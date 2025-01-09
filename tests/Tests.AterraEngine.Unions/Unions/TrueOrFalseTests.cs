// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TrueOrFalseTests {
    [Test]
    public async Task Test_UnionIsTrue() {
        // Arrange
        TrueOrFalse union = new True();

        // Act
        bool isTrue = union.IsTrue;
        bool isFalse = union.IsFalse;
        object? value = union.Value;

        // Assert
        await Assert.That(isTrue).IsTrue();
        await Assert.That(isFalse).IsFalse();
        await Assert.That(value).IsTypeOf<True>();
    }

    [Test]
    public async Task Test_UnionIsFalse() {
        // Arrange
        TrueOrFalse union = new False();

        // Act
        bool isTrue = union.IsTrue;
        bool isFalse = union.IsFalse;
        object? value = union.Value;

        // Assert
        await Assert.That(isTrue).IsFalse();
        await Assert.That(isFalse).IsTrue();
        await Assert.That(value).IsTypeOf<False>();
    }

    [Test]
    public async Task Test_ImplicitConversion_Bool_True() {
        // Arrange
        bool input = true;

        // Act
        TrueOrFalse union = input;

        // Assert
        await Assert.That(union.IsTrue).IsTrue();
        await Assert.That(union.IsFalse).IsFalse();
        await Assert.That(union.Value).IsTypeOf<True>();
    }

    [Test]
    public async Task Test_ImplicitConversion_Bool_False() {
        // Arrange
        bool input = false;

        // Act
        TrueOrFalse union = input;

        // Assert
        await Assert.That(union.IsTrue).IsFalse();
        await Assert.That(union.IsFalse).IsTrue();
        await Assert.That(union.Value).IsTypeOf<False>();
    }

    [Test]
    public async Task Test_ImplicitConversion_TrueToBool() {
        // Arrange
        TrueOrFalse union = new True();

        // Act
        bool result = union;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Test_ImplicitConversion_FalseToBool() {
        // Arrange
        TrueOrFalse union = new False();

        // Act
        bool result = union;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Test_TryGetAsTrue_Success() {
        // Arrange
        TrueOrFalse union = new True();

        // Act
        bool result = union.TryGetAsTrue(out True value);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(value).IsNotNull();
    }

    [Test]
    public async Task Test_TryGetAsTrue_Failure() {
        // Arrange
        TrueOrFalse union = new False();

        // Act
        bool result = union.TryGetAsTrue(out True value);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(value).IsEqualTo(new True());
    }

    [Test]
    public async Task Test_TryGetAsFalse_Success() {
        // Arrange
        TrueOrFalse union = new False();

        // Act
        bool result = union.TryGetAsFalse(out False value);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(value).IsNotNull();
    }

    [Test]
    public async Task Test_TryGetAsFalse_Failure() {
        // Arrange
        TrueOrFalse union = new True();

        // Act
        bool result = union.TryGetAsFalse(out False value);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(value).IsEqualTo(new False());
    }

    [Test]
    public async Task Test_Match_TrueCase() {
        // Arrange
        TrueOrFalse union = new True();

        // Act
        string result = union.Match(
            _ => "SUCCESS",
            _ => "FAILURE"
        );

        // Assert
        await Assert.That(result).IsEqualTo("SUCCESS");
    }

    [Test]
    public async Task Test_Match_FalseCase() {
        // Arrange
        TrueOrFalse union = new False();

        // Act
        string result = union.Match(
            _ => "SUCCESS",
            _ => "FAILURE"
        );

        // Assert
        await Assert.That(result).IsEqualTo("FAILURE");
    }

    [Test]
    public async Task Test_MatchAsync_TrueCase() {
        // Arrange
        TrueOrFalse union = new True();

        // Act
        string result = await union.MatchAsync(
            async _ => await Task.FromResult("SUCCESS"),
            async _ => await Task.FromResult("FAILURE")
        );

        // Assert
        await Assert.That(result).IsEqualTo("SUCCESS");
    }

    [Test]
    public async Task Test_MatchAsync_FalseCase() {
        // Arrange
        TrueOrFalse union = new False();

        // Act
        string result = await union.MatchAsync(
            async _ => await Task.FromResult("SUCCESS"),
            async _ => await Task.FromResult("FAILURE")
        );

        // Assert
        await Assert.That(result).IsEqualTo("FAILURE");
    }

    [Test]
    public async Task Test_Switch_TrueCase() {
        // Arrange
        TrueOrFalse union = new True();

        // Act
        string result = string.Empty;

        union.Switch(
            _ => result = "TRUE",
            _ => result = "FALSE"
        );

        // Assert
        await Assert.That(result).IsEqualTo("TRUE");
    }

    [Test]
    public async Task Test_Switch_FalseCase() {
        // Arrange
        TrueOrFalse union = new False();

        // Act
        string result = string.Empty;

        union.Switch(
            _ => result = "TRUE",
            _ => result = "FALSE"
        );

        // Assert
        await Assert.That(result).IsEqualTo("FALSE");
    }

    [Test]
    public async Task Test_SwitchAsync_TrueCase() {
        // Arrange
        TrueOrFalse union = new True();

        // Act
        string result = string.Empty;

        await union.SwitchAsync(
            async _ => result = await Task.FromResult("TRUE"),
            async _ => result = await Task.FromResult("FALSE")
        );

        // Assert
        await Assert.That(result).IsEqualTo("TRUE");
    }

    [Test]
    public async Task Test_SwitchAsync_FalseCase() {
        // Arrange
        TrueOrFalse union = new False();

        // Act
        string result = string.Empty;

        await union.SwitchAsync(
            async _ => result = await Task.FromResult("TRUE"),
            async _ => result = await Task.FromResult("FALSE")
        );

        // Assert
        await Assert.That(result).IsEqualTo("FALSE");
    }
}