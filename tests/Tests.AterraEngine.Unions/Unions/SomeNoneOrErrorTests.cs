// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SomeNoneOrErrorTests {
    [Test]
    public async Task Test_UnionIsSome() {
        // Arrange
        SomeNoneOrError<int, string> union = new Some<int>([1, 2, 3]);

        // Act
        bool isSome = union.IsSome;
        bool isNone = union.IsNone;
        bool isError = union.IsError;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsTrue();
        await Assert.That(isNone).IsFalse();
        await Assert.That(isError).IsFalse();
        await Assert.That(value).IsTypeOf<Some<int>>();
    }

    [Test]
    public async Task Test_UnionIsNone() {
        // Arrange
        SomeNoneOrError<int, string> union = new None();

        // Act
        bool isSome = union.IsSome;
        bool isNone = union.IsNone;
        bool isError = union.IsError;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsFalse();
        await Assert.That(isNone).IsTrue();
        await Assert.That(isError).IsFalse();
        await Assert.That(value).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_UnionIsError() {
        // Arrange
        SomeNoneOrError<int, string> union = new Error<string>("Error message");

        // Act
        bool isSome = union.IsSome;
        bool isNone = union.IsNone;
        bool isError = union.IsError;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsFalse();
        await Assert.That(isNone).IsFalse();
        await Assert.That(isError).IsTrue();
        await Assert.That(value).IsTypeOf<Error<string>>();
    }

    [Test]
    public async Task Test_TryGetAsSome_Success() {
        // Arrange
        SomeNoneOrError<int, string> union = new Some<int>([1, 2, 3]);

        // Act
        bool success = union.TryGetAsSome(out Some<int> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<Some<int>>();
        await Assert.That(result.Values).IsEquivalentTo([1, 2, 3]);
    }

    [Test]
    public async Task Test_TryGetAsNone_Success() {
        // Arrange
        SomeNoneOrError<int, string> union = new None();

        // Act
        bool success = union.TryGetAsNone(out None result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_TryGetAsError_Success() {
        // Arrange
        SomeNoneOrError<int, string> union = new Error<string>("Error message");

        // Act
        bool success = union.TryGetAsError(out Error<string> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<Error<string>>();
        await Assert.That(result.Value).IsEqualTo("Error message");
    }

    [Test]
    public async Task Test_Match_Some() {
        // Arrange
        SomeNoneOrError<int, string> union = new Some<int>([1, 2, 3]);

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            _ => -1,
            _ => -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(6);
    }

    [Test]
    public async Task Test_Match_None() {
        // Arrange
        SomeNoneOrError<int, string> union = new None();

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            _ => -1,
            _ => -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_Match_Error() {
        // Arrange
        SomeNoneOrError<int, string> union = new Error<string>("Error");

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            _ => -1,
            _ => -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(-2);
    }

    [Test]
    public async Task Test_Switch_Some() {
        // Arrange
        SomeNoneOrError<int, string> union = new Some<int>([1, 2, 3]);

        // Act
        int? result = null;

        union.Switch(
            some => result = some.Values.Sum(),
            _ => result = -1,
            _ => result = -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(6);
    }

    [Test]
    public async Task Test_Switch_None() {
        // Arrange
        SomeNoneOrError<int, string> union = new None();

        // Act
        int? result = null;

        union.Switch(
            some => result = some.Values.Sum(),
            _ => result = -1,
            _ => result = -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_Switch_Error() {
        // Arrange
        SomeNoneOrError<int, string> union = new Error<string>("Error");

        // Act
        int? result = null;

        union.Switch(
            some => result = some.Values.Sum(),
            _ => result = -1,
            _ => result = -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(-2);
    }

    [Test]
    public async Task Test_MatchAsync_Some() {
        // Arrange
        SomeNoneOrError<int, string> union = new Some<int>([1, 2, 3]);

        // Act
        int result = await union.MatchAsync(
            async some => await Task.FromResult(some.Values.Sum()),
            async _ => await Task.FromResult(-1),
            async _ => await Task.FromResult(-2)
        );

        // Assert
        await Assert.That(result).IsEqualTo(6);
    }

    [Test]
    public async Task Test_MatchAsync_None() {
        // Arrange
        SomeNoneOrError<int, string> union = new None();

        // Act
        int result = await union.MatchAsync(
            async some => await Task.FromResult(some.Values.Sum()),
            async _ => await Task.FromResult(-1),
            async _ => await Task.FromResult(-2)
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_MatchAsync_Error() {
        // Arrange
        SomeNoneOrError<int, string> union = new Error<string>("Error");

        // Act
        int result = await union.MatchAsync(
            async some => await Task.FromResult(some.Values.Sum()),
            async _ => await Task.FromResult(-1),
            async _ => await Task.FromResult(-2)
        );

        // Assert
        await Assert.That(result).IsEqualTo(-2);
    }
}