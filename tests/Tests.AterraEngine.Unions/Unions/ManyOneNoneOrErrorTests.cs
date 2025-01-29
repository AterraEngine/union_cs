// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ManyOneNoneOrErrorTests {
    [Test]
    public async Task Test_UnionHasMany() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new Many<int>([1, 2, 3]);

        // Act & Assert
        await Assert.That(union.IsMany).IsTrue();
        await Assert.That(union.IsOne).IsFalse();
        await Assert.That(union.IsNone).IsFalse();
        await Assert.That(union.IsError).IsFalse();
        await Assert.That(union.Value).IsTypeOf<Many<int>>();
    }

    [Test]
    public async Task Test_UnionHasOne() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new One<int>(42);

        // Act & Assert
        await Assert.That(union.IsMany).IsFalse();
        await Assert.That(union.IsOne).IsTrue();
        await Assert.That(union.IsNone).IsFalse();
        await Assert.That(union.IsError).IsFalse();
        await Assert.That(union.Value).IsTypeOf<One<int>>();
    }

    [Test]
    public async Task Test_UnionHasNone() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new None();

        // Act & Assert
        await Assert.That(union.IsMany).IsFalse();
        await Assert.That(union.IsOne).IsFalse();
        await Assert.That(union.IsNone).IsTrue();
        await Assert.That(union.IsError).IsFalse();
        await Assert.That(union.Value).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_UnionHasError() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new Error<string>("An error occurred");

        // Act & Assert
        await Assert.That(union.IsMany).IsFalse();
        await Assert.That(union.IsOne).IsFalse();
        await Assert.That(union.IsNone).IsFalse();
        await Assert.That(union.IsError).IsTrue();
        await Assert.That(union.Value).IsTypeOf<Error<string>>();
    }

    [Test]
    public async Task Test_TryGetAsMany_Success() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new Many<int>([1, 2, 3]);

        // Act
        bool success = union.TryGetAsMany(out Many<int> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<Many<int>>();
    }

    [Test]
    public async Task Test_TryGetAsOne_Success() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new One<int>(42);

        // Act
        bool success = union.TryGetAsOne(out One<int> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<One<int>>();
    }

    [Test]
    public async Task Test_TryGetAsNone_Success() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new None();

        // Act
        bool success = union.TryGetAsNone(out None result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_TryGetAsError_Success() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new Error<string>("An error occurred");

        // Act
        bool success = union.TryGetAsError(out Error<string> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<Error<string>>();
    }

    [Test]
    public async Task Test_TryGetAsMany_Failure() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new None();

        // Act
        bool success = union.TryGetAsMany(out Many<int> result);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That((object)result).IsTypeOf<Many<int>>();
        await Assert.That(result).IsEqualTo(default);
    }

    [Test]
    public async Task Test_TryGetAsOne_Failure() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new Many<int>([1, 2, 3]);

        // Act
        bool success = union.TryGetAsOne(out One<int> result);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That((object)result).IsTypeOf<One<int>>();
        await Assert.That(result).IsEqualTo(default);
    }

    [Test]
    public async Task Test_SwitchCase_Many() {
        // Arrange
        var many = new Many<int>([1, 2, 3]);
        ManyOneNoneOrError<int, string> union = many;

        // Act & Assert
        switch (union) {
            case { IsMany: true, AsMany: var manyValue }:
                await Assert.That((object)manyValue).IsTypeOf<Many<int>>();
                await Assert.That(manyValue).IsEqualTo(many);
                break;
            case { IsOne: true }:
                Assert.Fail("Expected Many but got One");
                break;
            case { IsNone: true }:
                Assert.Fail("Expected Many but got None");
                break;
            case { IsError: true }:
                Assert.Fail("Expected Many but got Error");
                break;
        }
    }

    [Test]
    public async Task Test_Match_Many() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new Many<int>([1, 2, 3]);

        // Act
        int result = union.Match(
            many => many.Values.Sum(),
            one => one.Value,
            _ => 0,
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(6);
    }

    [Test]
    public async Task Test_Match_One() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new One<int>(42);

        // Act
        int result = union.Match(
            many => many.Values.Sum(),
            one => one.Value,
            _ => 0,
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(42);
    }

    [Test]
    public async Task Test_Match_None() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new None();

        // Act
        int result = union.Match(
            many => many.Values.Sum(),
            one => one.Value,
            _ => 0,
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task Test_Match_Error() {
        // Arrange
        ManyOneNoneOrError<int, string> union = new Error<string>("An error occurred");

        // Act
        int result = union.Match(
            many => many.Values.Sum(),
            one => one.Value,
            _ => 0,
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }
}