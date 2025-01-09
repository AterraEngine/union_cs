// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SomeOrNoneTests {
    [Test]
    public async Task Test_UnionIsSome() {
        // Arrange
        SomeOrNone<int> union = new Some<int>([1, 2, 3]);

        // Act
        bool isSome = union.IsSome;
        bool isNone = union.IsNone;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsTrue();
        await Assert.That(isNone).IsFalse();
        await Assert.That(value).IsTypeOf<Some<int>>();
        await Assert.That(((Some<int>)value!).Values).IsEquivalentTo([1, 2, 3]);
    }

    [Test]
    public async Task Test_UnionIsNone() {
        // Arrange
        SomeOrNone<int> union = new None();

        // Act
        bool isSome = union.IsSome;
        bool isNone = union.IsNone;
        object? value = union.Value;

        // Assert
        await Assert.That(isSome).IsFalse();
        await Assert.That(isNone).IsTrue();
        await Assert.That(value).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_ImplicitConversion_ArrayToNone_NullArray() {
        // Arrange
        int[]? array = null;

        // Act
        SomeOrNone<int> union = array;

        // Assert
        await Assert.That(union.IsSome).IsFalse();
        await Assert.That(union.IsNone).IsTrue();
        await Assert.That(union.Value).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_ImplicitConversion_ArrayToNone_EmptyArray() {
        // Arrange
        int[] array = [
        ];

        // Act
        SomeOrNone<int> union = array;

        // Assert
        await Assert.That(union.IsSome).IsFalse();
        await Assert.That(union.IsNone).IsTrue();
        await Assert.That(union.Value).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_ImplicitConversion_ArrayToSome_NonEmptyArray() {
        // Arrange
        int[] array = [1, 2, 3];

        // Act
        SomeOrNone<int> union = array;

        // Assert
        await Assert.That(union.IsSome).IsTrue();
        await Assert.That(union.IsNone).IsFalse();
        await Assert.That(((Some<int>)union.Value!).Values).IsEquivalentTo([1, 2, 3]);
    }

    [Test]
    public async Task Test_TryGetAsSome() {
        // Arrange
        SomeOrNone<int> union = new Some<int>([4, 5, 6]);

        // Act
        bool result = union.TryGetAsSome(out Some<int> some);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(some).IsTypeOf<Some<int>>();
        await Assert.That(some.Values).IsEquivalentTo([4, 5, 6]);
    }

    [Test]
    public async Task Test_TryGetAsSome_Fail() {
        // Arrange
        SomeOrNone<int> union = new None();

        // Act
        bool result = union.TryGetAsSome(out Some<int> some);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(some).IsEqualTo(new Some<int>());
    }

    [Test]
    public async Task Test_TryGetAsNone() {
        // Arrange
        SomeOrNone<int> union = new None();

        // Act
        bool result = union.TryGetAsNone(out None none);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(none).IsTypeOf<None>();
    }

    [Test]
    public async Task Test_TryGetAsNone_Fail() {
        // Arrange
        SomeOrNone<int> union = new Some<int>([7, 8]);

        // Act
        bool result = union.TryGetAsNone(out None none);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(none).IsEqualTo(new None());
    }

    [Test]
    public async Task Test_Match_Some() {
        // Arrange
        SomeOrNone<int> union = new Some<int>([10, 20, 30]);

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(60);
    }

    [Test]
    public async Task Test_Match_None() {
        // Arrange
        SomeOrNone<int> union = new None();

        // Act
        int result = union.Match(
            some => some.Values.Sum(),
            _ => -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_MatchAsync_Some() {
        // Arrange
        SomeOrNone<int> union = new Some<int>([10, 20, 30]);

        // Act
        int result = await union.MatchAsync(
            async some => await Task.FromResult(some.Values.Sum()),
            async _ => await Task.FromResult(-1)
        );

        // Assert
        await Assert.That(result).IsEqualTo(60);
    }

    [Test]
    public async Task Test_MatchAsync_None() {
        // Arrange
        SomeOrNone<int> union = new None();

        // Act
        int result = await union.MatchAsync(
            async some => await Task.FromResult(some.Values.Sum()),
            async _ => await Task.FromResult(-1)
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_Switch_Some() {
        // Arrange
        SomeOrNone<int> union = new Some<int>([10, 20]);

        // Act
        int result = 0;

        union.Switch(
            some => result = some.Values.Sum(),
            _ => result = -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(30);
    }

    [Test]
    public async Task Test_Switch_None() {
        // Arrange
        SomeOrNone<int> union = new None();

        // Act
        int result = 0;

        union.Switch(
            some => result = some.Values.Sum(),
            _ => result = -1
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    [Test]
    public async Task Test_SwitchAsync_Some() {
        // Arrange
        SomeOrNone<int> union = new Some<int>([100, 200]);

        // Act
        int result = 0;

        await union.SwitchAsync(
            async some => result = await Task.FromResult(some.Values.Sum()),
            async _ => result = await Task.FromResult(-1)
        );

        // Assert
        await Assert.That(result).IsEqualTo(300);
    }

    [Test]
    public async Task Test_SwitchAsync_None() {
        // Arrange
        SomeOrNone<int> union = new None();

        // Act
        int result = 0;

        await union.SwitchAsync(
            async some => result = await Task.FromResult(some.Values.Sum()),
            async _ => result = await Task.FromResult(-1)
        );

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }
}