// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SomeOneOrNoneTests {
    [Test]
    public async Task Test_UnionIsSome() {
        // Arrange
        SomeOneOrNone<int> union = new Some<int>([1, 2, 3]);

        // Act
        bool isSome = union.IsSome;
        bool isOne = union.IsOne;
        bool isNone = union.IsNone;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsTrue();
        await Assert.That(isOne).IsFalse();
        await Assert.That(isNone).IsFalse();
        await Assert.That(value).IsTypeOf<Some<int>>();
        await Assert.That(((Some<int>)value!).Values).IsEquivalentTo([1, 2, 3]);
    }

    [Test]
    public async Task Test_UnionIsOne() {
        // Arrange
        SomeOneOrNone<int> union = new One<int>(42);

        // Act
        bool isSome = union.IsSome;
        bool isOne = union.IsOne;
        bool isNone = union.IsNone;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsFalse();
        await Assert.That(isOne).IsTrue();
        await Assert.That(isNone).IsFalse();
        await Assert.That(value).IsTypeOf<One<int>>();
        await Assert.That(((One<int>)value!).Value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_UnionIsNone() {
        // Arrange
        SomeOneOrNone<int> union = new None();

        // Act
        bool isSome = union.IsSome;
        bool isOne = union.IsOne;
        bool isNone = union.IsNone;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsFalse();
        await Assert.That(isOne).IsFalse();
        await Assert.That(isNone).IsTrue();
        await Assert.That(value).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_ImplicitConversion_ArrayToNone() {
        // Arrange
        int[]? array = null;

        // Act
        SomeOneOrNone<int> union = array;

        // Assert
        await Assert.That(union.IsNone).IsTrue();
        await Assert.That(union.IsSome).IsFalse();
        await Assert.That(union.IsOne).IsFalse();
        await Assert.That(union.Value).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_ImplicitConversion_ArrayToOne() {
        // Arrange
        int[] array = [42];

        // Act
        SomeOneOrNone<int> union = array;

        // Assert
        await Assert.That(union.IsOne).IsTrue();
        await Assert.That(union.IsSome).IsFalse();
        await Assert.That(union.IsNone).IsFalse();
        await Assert.That(((One<int>)union.Value!).Value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_ImplicitConversion_ArrayToSome() {
        // Arrange
        int[] array = [1, 2, 3];

        // Act
        SomeOneOrNone<int> union = array;

        // Assert
        await Assert.That(union.IsSome).IsTrue();
        await Assert.That(union.IsOne).IsFalse();
        await Assert.That(union.IsNone).IsFalse();
        await Assert.That(((Some<int>)union.Value!).Values).IsEquivalentTo([1, 2, 3]);
    }

    [Test]
    public async Task Test_TryGetAsSome() {
        // Arrange
        SomeOneOrNone<int> union = new Some<int>([1, 2, 3]);

        // Act
        bool success = union.TryGetAsSome(out Some<int> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<Some<int>>();
        await Assert.That(result.Values).IsEquivalentTo([1, 2, 3]);
    }

    [Test]
    public async Task Test_TryGetAsOne() {
        // Arrange
        SomeOneOrNone<int> union = new One<int>(42);

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
        SomeOneOrNone<int> union = new None();

        // Act
        bool success = union.TryGetAsNone(out None result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_Match_Some() {
        // Arrange
        SomeOneOrNone<int> union = new Some<int>([1, 2, 3]);

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            one => one.Value,
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(6);
    }

    [Test]
    public async Task Test_Match_One() {
        // Arrange
        SomeOneOrNone<int> union = new One<int>(42);

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            one => one.Value,
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(42);
    }

    [Test]
    public async Task Test_Match_None() {
        // Arrange
        SomeOneOrNone<int> union = new None();

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            one => one.Value,
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_Switch_Some() {
        // Arrange
        SomeOneOrNone<int> union = new Some<int>([10, 20]);

        // Act
        int? result = null;

        union.Switch(
            some => result = some.Values.Sum(),
            one => result = one.Value,
            _ => result = -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(30);
    }

    [Test]
    public async Task Test_Switch_One() {
        // Arrange
        SomeOneOrNone<int> union = new One<int>(5);

        // Act
        int? result = null;

        union.Switch(
            some => result = some.Values.Sum(),
            one => result = one.Value,
            _ => result = -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(5);
    }

    [Test]
    public async Task Test_Switch_None() {
        // Arrange
        SomeOneOrNone<int> union = new None();

        // Act
        int? result = null;

        union.Switch(
            some => result = some.Values.Sum(),
            one => result = one.Value,
            _ => result = -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }
}