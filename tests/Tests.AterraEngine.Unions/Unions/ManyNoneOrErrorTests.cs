// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ManyNoneOrErrorTests {
    [Test]
    public async Task Test_UnionHasMany() {
        ManyNoneOrError<int, string> union = new Many<int>([1, 2, 3]);

        await Assert.That(union.IsMany).IsTrue();
        await Assert.That(union.IsNone).IsFalse();
        await Assert.That(union.IsError).IsFalse();
        await Assert.That(union.Value).IsTypeOf<Many<int>>();
    }

    [Test]
    public async Task Test_UnionHasNone() {
        ManyNoneOrError<int, string> union = new None();

        await Assert.That(union.IsMany).IsFalse();
        await Assert.That(union.IsNone).IsTrue();
        await Assert.That(union.IsError).IsFalse();
        await Assert.That(union.Value).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_UnionHasError() {
        ManyNoneOrError<int, string> union = new Error<string>("An error occurred");

        await Assert.That(union.IsMany).IsFalse();
        await Assert.That(union.IsNone).IsFalse();
        await Assert.That(union.IsError).IsTrue();
        await Assert.That(union.Value).IsTypeOf<Error<string>>();
    }

    [Test]
    public async Task Test_TryGetAsMany_Success() {
        ManyNoneOrError<int, string> union = new Many<int>([1, 2, 3]);

        await Assert.That(union.TryGetAsMany(out Many<int> result)).IsTrue();
        await Assert.That(result).IsTypeOf<Many<int>>();
    }

    [Test]
    public async Task Test_TryGetAsNone_Success() {
        ManyNoneOrError<int, string> union = new None();

        await Assert.That(union.TryGetAsNone(out None result)).IsTrue();
        await Assert.That(result).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_TryGetAsError_Success() {
        ManyNoneOrError<int, string> union = new Error<string>("An error occurred");

        await Assert.That(union.TryGetAsError(out Error<string> result)).IsTrue();
        await Assert.That(result).IsTypeOf<Error<string>>();
    }

    [Test]
    public async Task Test_TryGetAsMany_Failure() {
        ManyNoneOrError<int, string> union = new None();

        await Assert.That(union.TryGetAsMany(out Many<int> result)).IsFalse();
        await Assert.That(result).IsTypeOf<Many<int>>()
            .And.IsEqualTo(default);
    }

    [Test]
    public async Task Test_TryGetAsNone_Failure() {
        ManyNoneOrError<int, string> union = new Error<string>("An error occurred");

        await Assert.That(union.TryGetAsNone(out None result)).IsFalse();
        await Assert.That(result).IsTypeOf<None>()
            .And.IsEqualTo(default);
    }

    [Test]
    public async Task Test_TryGetAsError_Failure() {
        ManyNoneOrError<int, string> union = new Many<int>([4, 5, 6]);

        await Assert.That(union.TryGetAsError(out Error<string> result)).IsFalse();
        await Assert.That(result).IsTypeOf<Error<string>>()
            .And.IsEqualTo(default);
    }

    [Test]
    public async Task Test_ImplicitConversion_Many() {
        var value = new Many<int>([1, 2, 3]);
        ManyNoneOrError<int, string> union = value;

        await Assert.That(union.IsMany).IsTrue();
        await Assert.That(union.IsNone).IsFalse();
        await Assert.That(union.IsError).IsFalse();
        await Assert.That(union.AsMany).IsTypeOf<Many<int>>();
    }

    [Test]
    public async Task Test_ImplicitConversion_None() {
        var value = new None();
        ManyNoneOrError<int, string> union = value;

        await Assert.That(union.IsMany).IsFalse();
        await Assert.That(union.IsNone).IsTrue();
        await Assert.That(union.IsError).IsFalse();
        await Assert.That(union.AsNone).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_ImplicitConversion_Error() {
        var value = new Error<string>("An error occurred");
        ManyNoneOrError<int, string> union = value;

        await Assert.That(union.IsMany).IsFalse();
        await Assert.That(union.IsNone).IsFalse();
        await Assert.That(union.IsError).IsTrue();
        await Assert.That(union.AsError).IsTypeOf<Error<string>>();
    }

    [Test]
    public async Task Test_SwitchCase_Many() {
        // Arrange
        var many = new Many<int>([1, 2, 3]);
        ManyNoneOrError<int, string> union = many;

        // Act & Assert
        switch (union) {
            case { IsMany: true, AsMany: var manyValue }:
                await Assert.That(manyValue).IsTypeOf<Many<int>>().And.IsEqualTo(many);
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
    public async Task Test_SwitchCase_None() {
        // Arrange
        var none = new None();
        ManyNoneOrError<int, string> union = none;

        // Act & Assert
        switch (union) {
            case { IsMany: true }:
                Assert.Fail("Expected None but got Many");
                break;
            case { IsNone: true, AsNone: var noneValue }:
                await Assert.That(noneValue).IsTypeOf<None>().And.IsEqualTo(none);
                break;
            case { IsError: true }:
                Assert.Fail("Expected None but got Error");
                break;
        }
    }

    [Test]
    public async Task Test_SwitchCase_Error() {
        // Arrange
        var error = new Error<string>("An error occurred");
        ManyNoneOrError<int, string> union = error;

        // Act & Assert
        switch (union) {
            case { IsMany: true }:
                Assert.Fail("Expected Error but got Many");
                break;
            case { IsNone: true }:
                Assert.Fail("Expected Error but got None");
                break;
            case { IsError: true, AsError: var errorValue }:
                await Assert.That(errorValue).IsTypeOf<Error<string>>().And.IsEqualTo(error);
                break;
        }
    }

    [Test]
    public async Task Test_Match_Many() {
        ManyNoneOrError<int, string> union = new Many<int>([1, 2, 3]);

        int result = union.Match(
            many => many.Values.Sum(),
            _ => 0,
            _ => -1
        );

        await Assert.That(result).IsEqualTo(6);
    }

    [Test]
    public async Task Test_Match_None() {
        ManyNoneOrError<int, string> union = new None();

        int result = union.Match(
            many => many.Values.Sum(),
            _ => 0,
            _ => -1
        );

        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task Test_Match_Error() {
        ManyNoneOrError<int, string> union = new Error<string>("An error occurred");

        int result = union.Match(
            many => many.Values.Sum(),
            _ => 0,
            _ => -1
        );

        await Assert.That(result).IsEqualTo(-1);
    }
}