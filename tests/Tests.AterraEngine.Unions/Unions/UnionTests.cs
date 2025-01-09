// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionTests
{
    [Test]
    public async Task Test_SingleTypeUnion_Success()
    {
        // Arrange
        Union<int> union = 42;

        // Act
        bool isT0 = union.IsT0;
        object? value = union.Value;

        // Assert
        await Assert.That(isT0).IsTrue();
        await Assert.That(value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_DoubleTypeUnion_FirstType()
    {
        // Arrange
        Union<int, string> union = 42;

        // Act
        bool isT0 = union.IsT0;
        bool isT1 = union.IsT1;
        object? value = union.Value;

        // Assert
        await Assert.That(isT0).IsTrue();
        await Assert.That(isT1).IsFalse();
        await Assert.That(value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_DoubleTypeUnion_SecondType()
    {
        // Arrange
        Union<int, string> union = "Hello";

        // Act
        bool isT0 = union.IsT0;
        bool isT1 = union.IsT1;
        object? value = union.Value;

        // Assert
        await Assert.That(isT0).IsFalse();
        await Assert.That(isT1).IsTrue();
        await Assert.That(value).IsEqualTo("Hello");
    }

    [Test]
    public async Task Test_TripleTypeUnion_ThirdType()
    {
        // Arrange
        Union<int, string, bool> union = true;

        // Act
        bool isT0 = union.IsT0;
        bool isT1 = union.IsT1;
        bool isT2 = union.IsT2;
        object? value = union.Value;

        // Assert
        await Assert.That(isT0).IsFalse();
        await Assert.That(isT1).IsFalse();
        await Assert.That(isT2).IsTrue();
        await Assert.That(value).IsEqualTo(true);
    }

    [Test]
    public async Task Test_TryGetAsExactType_FirstType()
    {
        // Arrange
        Union<int, string> union = 123;

        // Act
        bool result = union.TryGetAsT0(out int value);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(value).IsEqualTo(123);
    }

    [Test]
    public async Task Test_TryGetAsExactType_InvalidType()
    {
        // Arrange
        Union<int, string> union = "Test";

        // Act
        bool result = union.TryGetAsT0(out int value);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(value).IsEqualTo(0);
    }

    [Test]
    public async Task Test_Match_SimpleResult()
    {
        // Arrange
        Union<int, string, bool> union1 = 42;
        Union<int, string, bool> union2 = "Test";
        Union<int, string, bool> union3 = false;

        // Act
        int result1 = union1.Match(
            t0 => t0 * 2,
            t1 => t1!.Length,
            t2 => t2 ? 1 : 0
        );

        int result2 = union2.Match(
            t0 => t0 * 2,
            t1 => t1!.Length,
            t2 => t2 ? 1 : 0
        );

        int result3 = union3.Match(
            t0 => t0 * 2,
            t1 => t1!.Length,
            t2 => t2 ? 1 : 0
        );

        // Assert
        await Assert.That(result1).IsEqualTo(84);
        await Assert.That(result2).IsEqualTo(4);
        await Assert.That(result3).IsEqualTo(0);
    }

    [Test]
    public async Task Test_MatchAsync_SimpleResult()
    {
        // Arrange
        Union<int, string, bool> union = "TestString";

        // Act
        int result = await union.MatchAsync(
            async t0 => await Task.FromResult(t0 * 3),
            async t1 => await Task.FromResult(t1!.Length),
            async t2 => await Task.FromResult(t2 ? 1 : 0)
        );

        // Assert
        await Assert.That(result).IsEqualTo(10);
    }

    [Test]
    public async Task Test_Switch_Action_CorrectCaseTriggered()
    {
        // Arrange
        Union<int, string, bool> union = 10;

        string output = string.Empty;

        // Act
        union.Switch(
            _ => output = "Case1",
            _ => output = "Case2",
            _ => output = "Case3"
        );

        // Assert
        await Assert.That(output).IsEqualTo("Case1");
    }

    [Test]
    public async Task Test_SwitchAsync_ActionAsync_CorrectCaseTriggered()
    {
        // Arrange
        Union<int, string, bool> union = "StringCase";

        string output = string.Empty;

        // Act
        await union.SwitchAsync(
            async _ => output = await Task.FromResult("Case1"),
            async _ => output = await Task.FromResult("Case2"),
            async _ => output = await Task.FromResult("Case3")
        );

        // Assert
        await Assert.That(output).IsEqualTo("Case2");
    }

    [Test]
    public async Task Test_InvalidUnion_NoValue_ThrowsException()
    {
        // Arrange
        bool exceptionResult = false;

        // Act
        try
        {
            Union<int, string> union = default;
            object? _ = union.Value; // Force exception
        }
        catch (ArgumentException)
        {
            exceptionResult = true;
        }

        // Assert
        await Assert.That(exceptionResult).IsTrue();
    }
}