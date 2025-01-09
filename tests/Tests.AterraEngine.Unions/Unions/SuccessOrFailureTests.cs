// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SuccessOrFailureTests {
    [Test]
    public async Task Test_UnionIsSuccess() {
        // Arrange
        SuccessOrFailure<int, string> union = new Success<int>(42);

        // Act
        bool isSuccess = union.IsSuccess;
        bool isFailure = union.IsFailure;
        object? value = union.Value;

        // Assert
        await Assert.That(isSuccess).IsTrue();
        await Assert.That(isFailure).IsFalse();
        await Assert.That(value).IsTypeOf<Success<int>>();
        await Assert.That(((Success<int>)value!).Value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_UnionIsFailure() {
        // Arrange
        SuccessOrFailure<int, string> union = new Failure<string>("Error");

        // Act
        bool isSuccess = union.IsSuccess;
        bool isFailure = union.IsFailure;
        object? value = union.Value;

        // Assert
        await Assert.That(isSuccess).IsFalse();
        await Assert.That(isFailure).IsTrue();
        await Assert.That(value).IsTypeOf<Failure<string>>();
        await Assert.That(((Failure<string>)value!).Value).IsEqualTo("Error");
    }

    [Test]
    public async Task Test_ImplicitConversion_Success() {
        // Arrange
        int successValue = 42;

        // Act
        SuccessOrFailure<int, string> union = successValue;

        // Assert
        await Assert.That(union.IsSuccess).IsTrue();
        await Assert.That(union.IsFailure).IsFalse();
        await Assert.That(((Success<int>)union.Value!).Value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_ImplicitConversion_Failure() {
        // Arrange
        string failureValue = "Something went wrong";

        // Act
        SuccessOrFailure<int, string> union = failureValue;

        // Assert
        await Assert.That(union.IsSuccess).IsFalse();
        await Assert.That(union.IsFailure).IsTrue();
        await Assert.That(((Failure<string>)union.Value!).Value).IsEqualTo("Something went wrong");
    }

    [Test]
    public async Task Test_TryGetAsSuccessValue() {
        // Arrange
        SuccessOrFailure<int, string> union = new Success<int>(99);

        // Act
        bool result = union.TryGetAsSuccessValue(out int value);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(value!).IsEqualTo(99);
    }

    [Test]
    public async Task Test_TryGetAsSuccessValue_Fail() {
        // Arrange
        SuccessOrFailure<int, string> union = new Failure<string>("Failure!");

        // Act
        bool result = union.TryGetAsSuccessValue(out int value);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(value).IsEqualTo(0);
    }

    [Test]
    public async Task Test_TryGetAsFailureValue() {
        // Arrange
        SuccessOrFailure<int, string> union = new Failure<string>("Some Error");

        // Act
        bool result = union.TryGetAsFailureValue(out string? value);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(value!).IsEqualTo("Some Error");
    }

    [Test]
    public async Task Test_TryGetAsFailureValue_Fail() {
        // Arrange
        SuccessOrFailure<int, string> union = new Success<int>(101);

        // Act
        bool result = union.TryGetAsFailureValue(out string? value);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(value).IsNull();
    }

    [Test]
    public async Task Test_Match_Success() {
        // Arrange
        SuccessOrFailure<int, string> union = new Success<int>(20);

        // Act
        int result = union.Match(
            success => success.Value * 2, 
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(40);
    }

    [Test]
    public async Task Test_Match_Failure() {
        // Arrange
        SuccessOrFailure<int, string> union = new Failure<string>("No Value");

        // Act
        int result = union.Match(
            success => success.Value * 2, 
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_MatchAsync_Success() {
        // Arrange
        SuccessOrFailure<int, string> union = new Success<int>(50);

        // Act
        int result = await union.MatchAsync(
            async success => await Task.FromResult(success.Value + 10),
            async _ => await Task.FromResult(-1)
        );

        // Assert
        await Assert.That(result).IsEqualTo(60);
    }

    [Test]
    public async Task Test_MatchAsync_Failure() {
        // Arrange
        SuccessOrFailure<int, string> union = new Failure<string>("Always Fail");

        // Act
        int result = await union.MatchAsync(
            async success => await Task.FromResult(success.Value + 10),
            async _ => await Task.FromResult(-1)
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_Switch_Success() {
        // Arrange
        SuccessOrFailure<int, string> union = new Success<int>(15);

        // Act
        int result = 0;

        union.Switch(
            success => result = success.Value * 3, 
            _ => result = -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(45);
    }

    [Test]
    public async Task Test_Switch_Failure() {
        // Arrange
        SuccessOrFailure<int, string> union = new Failure<string>("Switch Test");

        // Act
        int result = 0;

        union.Switch(
            success => result = success.Value * 3, 
            _ => result = -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_SwitchAsync_Success() {
        // Arrange
        SuccessOrFailure<int, string> union = new Success<int>(35);

        // Act
        int result = 0;

        await union.SwitchAsync(
            async success => result = await Task.FromResult(success.Value + 5),
            async _ => result = await Task.FromResult(-1)
        );

        // Assert
        await Assert.That(result).IsEqualTo(40);
    }

    [Test]
    public async Task Test_SwitchAsync_Failure() {
        // Arrange
        SuccessOrFailure<int, string> union = new Failure<string>("Switch Async Test");

        // Act
        int result = 0;

        await union.SwitchAsync(
            async success => result = await Task.FromResult(success.Value + 5),
            async _ => result = await Task.FromResult(-1)
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }
}