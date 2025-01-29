// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SomeOneNoneOrErrorTests {
    [Test]
    public async Task Test_UnionIsSome() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new Some<int>([10, 20, 30]);

        // Act
        bool isSome = union.IsSome;
        bool isOne = union.IsOne;
        bool isNone = union.IsNone;
        bool isError = union.IsError;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsTrue();
        await Assert.That(isOne).IsFalse();
        await Assert.That(isNone).IsFalse();
        await Assert.That(isError).IsFalse();
        await Assert.That(value).IsTypeOf<Some<int>>();
        await Assert.That(((Some<int>)value!).Values).IsEquivalentTo([10, 20, 30]);
    }

    [Test]
    public async Task Test_UnionIsOne() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new One<int>(42);

        // Act
        bool isSome = union.IsSome;
        bool isOne = union.IsOne;
        bool isNone = union.IsNone;
        bool isError = union.IsError;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsFalse();
        await Assert.That(isOne).IsTrue();
        await Assert.That(isNone).IsFalse();
        await Assert.That(isError).IsFalse();
        await Assert.That(value).IsTypeOf<One<int>>();
        await Assert.That(((One<int>)value!).Value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_UnionIsNone() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new None();

        // Act
        bool isSome = union.IsSome;
        bool isOne = union.IsOne;
        bool isNone = union.IsNone;
        bool isError = union.IsError;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsFalse();
        await Assert.That(isOne).IsFalse();
        await Assert.That(isNone).IsTrue();
        await Assert.That(isError).IsFalse();
        await Assert.That(value).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_UnionIsError() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new Error<string>("Test Error");

        // Act
        bool isSome = union.IsSome;
        bool isOne = union.IsOne;
        bool isNone = union.IsNone;
        bool isError = union.IsError;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsFalse();
        await Assert.That(isOne).IsFalse();
        await Assert.That(isNone).IsFalse();
        await Assert.That(isError).IsTrue();
        await Assert.That(value).IsTypeOf<Error<string>>();
        await Assert.That(((Error<string>)value!).Value).IsEqualTo("Test Error");
    }

    [Test]
    public async Task Test_TryGetAsSome() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new Some<int>([10, 20, 30]);

        // Act
        bool success = union.TryGetAsSome(out Some<int> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<Some<int>>();
        await Assert.That(result.Values).IsEquivalentTo([10, 20, 30]);
    }

    [Test]
    public async Task Test_TryGetAsOne() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new One<int>(42);

        // Act
        bool success = union.TryGetAsOne(out One<int> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<One<int>>();
        await Assert.That(result.Value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_TryGetAsNone() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new None();

        // Act
        bool success = union.TryGetAsNone(out None result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_TryGetAsError() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new Error<string>("Example Error");

        // Act
        bool success = union.TryGetAsError(out Error<string> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<Error<string>>();
        await Assert.That(result.Value).IsEqualTo("Example Error");
    }

    [Test]
    public async Task Test_Match_Some() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new Some<int>([1, 2, 3]);

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            one => one.Value,
            _ => -1,
            _ => -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(6);
    }

    [Test]
    public async Task Test_Match_One() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new One<int>(42);

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            one => one.Value,
            _ => -1,
            _ => -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(42);
    }

    [Test]
    public async Task Test_Match_None() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new None();

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            one => one.Value,
            _ => -1,
            _ => -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_Match_Error() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new Error<string>("Test error");

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            one => one.Value,
            _ => -1,
            _ => -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(-2);
    }

    [Test]
    public async Task Test_Switch_Some() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new Some<int>([10, 20]);

        // Act
        int? result = null;

        union.Switch(
            some => result = some.Values.Sum(),
            one => result = one.Value,
            _ => result = -1,
            _ => result = -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(30);
    }

    [Test]
    public async Task Test_Switch_One() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new One<int>(5);

        // Act
        int? result = null;

        union.Switch(
            some => result = some.Values.Sum(),
            one => result = one.Value,
            _ => result = -1,
            _ => result = -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(5);
    }

    [Test]
    public async Task Test_Switch_None() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new None();

        // Act
        int? result = null;

        union.Switch(
            some => result = some.Values.Sum(),
            one => result = one.Value,
            _ => result = -1,
            _ => result = -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_Switch_Error() {
        // Arrange
        SomeOneNoneOrError<int, string> union = new Error<string>("Error!");

        // Act
        int? result = null;

        union.Switch(
            some => result = some.Values.Sum(),
            one => result = one.Value,
            _ => result = -1,
            _ => result = -2
        );

        // Assert
        await Assert.That(result).IsEqualTo(-2);
    }
}