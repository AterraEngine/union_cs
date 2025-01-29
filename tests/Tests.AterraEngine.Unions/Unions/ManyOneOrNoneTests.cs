// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ManyOneOrNoneTests {
    [Test]
    public async Task Test_UnionHasMany() {
        // Arrange
        ManyOneOrNone<int> union = new Many<int>([1, 2, 3]);

        // Act
        bool isMany = union.IsMany;
        bool isOne = union.IsOne;
        bool isNone = union.IsNone;
        object? value = union.Value;

        // Assert
        await Assert.That(isMany).IsTrue();
        await Assert.That(isOne).IsFalse();
        await Assert.That(isNone).IsFalse();
        await Assert.That(value).IsTypeOf<Many<int>>();
    }

    [Test]
    public async Task Test_UnionHasOne() {
        // Arrange
        ManyOneOrNone<int> union = new One<int>(42);

        // Act
        bool isMany = union.IsMany;
        bool isOne = union.IsOne;
        bool isNone = union.IsNone;
        object? value = union.Value;

        // Assert
        await Assert.That(isMany).IsFalse();
        await Assert.That(isOne).IsTrue();
        await Assert.That(isNone).IsFalse();
        await Assert.That(value).IsTypeOf<One<int>>();
    }

    [Test]
    public async Task Test_UnionHasNone() {
        // Arrange
        ManyOneOrNone<int> union = new None();

        // Act
        bool isMany = union.IsMany;
        bool isOne = union.IsOne;
        bool isNone = union.IsNone;
        object? value = union.Value;

        // Assert
        await Assert.That(isMany).IsFalse();
        await Assert.That(isOne).IsFalse();
        await Assert.That(isNone).IsTrue();
        await Assert.That(value).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_TryGetAsMany_Success() {
        // Arrange
        ManyOneOrNone<int> union = new Many<int>([1, 2, 3]);

        // Act
        bool success = union.TryGetAsMany(out Many<int> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<Many<int>>();
        await Assert.That(result.Values).IsEquivalentTo([1, 2, 3]);
    }

    [Test]
    public async Task Test_TryGetAsOne_Success() {
        // Arrange
        ManyOneOrNone<int> union = new One<int>(42);

        // Act
        bool success = union.TryGetAsOne(out One<int> result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<One<int>>();
        await Assert.That(result.Value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_TryGetAsNone_Success() {
        // Arrange
        ManyOneOrNone<int> union = new None();

        // Act
        bool success = union.TryGetAsNone(out None result);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That((object)result).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_TryGetAsMany_Failure() {
        // Arrange
        ManyOneOrNone<int> union = new None();

        // Act
        bool success = union.TryGetAsMany(out Many<int> result);

        // Assert
        await Assert.That(success).IsFalse();
        await Assert.That((object)result).IsTypeOf<Many<int>>();
        await Assert.That(result).IsEqualTo(default);
    }

    [Test]
    public async Task Test_ImplicitConversion_ArrayNull() {
        // Arrange
        int[]? array = null;

        // Act
        ManyOneOrNone<int> union = array;

        // Assert
        await Assert.That(union.IsMany).IsFalse();
        await Assert.That(union.IsOne).IsFalse();
        await Assert.That(union.IsNone).IsTrue();
    }

    [Test]
    public async Task Test_ImplicitConversion_ArrayEmpty() {
        // Arrange
        int[] array = [
        ];

        // Act
        ManyOneOrNone<int> union = array;

        // Assert
        await Assert.That(union.IsMany).IsFalse();
        await Assert.That(union.IsOne).IsFalse();
        await Assert.That(union.IsNone).IsTrue();
    }

    [Test]
    public async Task Test_ImplicitConversion_ArraySingleValue() {
        // Arrange
        int[] array = [42];

        // Act
        ManyOneOrNone<int> union = array;

        // Assert
        await Assert.That(union.IsMany).IsFalse();
        await Assert.That(union.IsOne).IsTrue();
        await Assert.That(union.AsOne.Value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_ImplicitConversion_ArrayMultipleValues() {
        // Arrange
        int[] array = [1, 2, 3];

        // Act
        ManyOneOrNone<int> union = array;

        // Assert
        await Assert.That(union.IsMany).IsTrue();
        await Assert.That(union.AsMany.Values).IsEqualTo(array);
    }

    [Test]
    public async Task Test_Match_Many() {
        // Arrange
        ManyOneOrNone<int> union = new Many<int>([1, 2, 3]);

        // Act
        int result = union.Match(
            many => many.Values.Sum(),
            one => one.Value,
            _ => 0
        );

        // Assert
        await Assert.That(result).IsEqualTo(6);
    }

    [Test]
    public async Task Test_Match_One() {
        // Arrange
        ManyOneOrNone<int> union = new One<int>(42);

        // Act
        int result = union.Match(
            many => many.Values.Sum(),
            one => one.Value,
            _ => 0
        );

        // Assert
        await Assert.That(result).IsEqualTo(42);
    }

    [Test]
    public async Task Test_Match_None() {
        // Arrange
        ManyOneOrNone<int> union = new None();

        // Act
        int result = union.Match(
            many => many.Values.Sum(),
            one => one.Value,
            _ => 0
        );

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task Test_Switch_Many() {
        // Arrange
        ManyOneOrNone<int> union = new Many<int>([1, 2, 3]);

        // Act
        int? result = null;

        union.Switch(
            many => result = many.Values.Sum(),
            one => result = one.Value,
            _ => result = 0
        );

        // Assert
        await Assert.That(result).IsEqualTo(6);
    }

    [Test]
    public async Task Test_Switch_None() {
        // Arrange
        ManyOneOrNone<int> union = new None();

        // Act
        int? result = null;

        union.Switch(
            many => result = many.Values.Sum(),
            one => result = one.Value,
            _ => result = 0
        );

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }
}