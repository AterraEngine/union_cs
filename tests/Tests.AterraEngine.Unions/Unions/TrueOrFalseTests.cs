// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace Tests.AterraEngine.Unions.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TrueOrFalseTests {
    [Test]
    public async Task Test_UnionHasTrue() {
        TrueOrFalse union = new True();
        
        await Assert.That(union.IsTrue).IsTrue();
        await Assert.That(union.IsFalse).IsFalse();
        await Assert.That(union.Value).IsTypeOf<True>();
    }

    [Test]
    public async Task Test_UnionHasFalse() {
        TrueOrFalse union = new False();
        
        await Assert.That(union.IsTrue).IsFalse();
        await Assert.That(union.IsFalse).IsTrue();
        await Assert.That(union.Value).IsTypeOf<False>();
    }

    [Test]
    public async Task Test_TryGetAsTrue_Success() {
        TrueOrFalse union = new True();
        
        await Assert.That(union.TryGetAsTrue(out True result)).IsTrue();
        await Assert.That(result).IsTypeOf<True>();
    }

    [Test]
    public async Task Test_TryGetAsFalse_Success() {
        TrueOrFalse union = new False();
        
        await Assert.That(union.TryGetAsFalse(out False result)).IsTrue();
        await Assert.That(result).IsTypeOf<False>();
    }

    [Test]
    public async Task Test_TryGetAsTrue_Failure() {
        TrueOrFalse union = new False();
        
        await Assert.That(union.TryGetAsTrue(out True result)).IsFalse();
        await Assert.That(result).IsTypeOf<True>()
            .And.IsEqualTo(default);
    }

    [Test]
    public async Task Test_TryGetAsFalse_Failure() {
        TrueOrFalse union = new True();
        
        await Assert.That(union.TryGetAsFalse(out False result)).IsFalse();
        await Assert.That(result).IsTypeOf<False>()
            .And.IsEqualTo(default);
    }

    [Test]
    public async Task Test_ImplicitConversion() {
        var value = new True();
        TrueOrFalse union = value;
        
        await Assert.That(union.IsTrue).IsTrue();
        await Assert.That(union.IsFalse).IsFalse();
        await Assert.That(union.Value).IsTypeOf<True>();
        await Assert.That(union.AsTrue).IsTypeOf<True>();
    }

    [Test]
    public async Task Test_SwitchCase_True() {
        TrueOrFalse union = new True();
        switch (union) {
            case {IsTrue: true, AsTrue: var trueValue}:
                await Assert.That(trueValue).IsTypeOf<True>().And.IsEqualTo(new True());
                break;
            case {IsFalse: true, AsFalse: var falseValue}: 
                await Assert.That(falseValue).IsTypeOf<False>().And.IsEqualTo(new False());
                break;
        }
    }

    [Test]
    public async Task Test_SwitchCase_False() {
        TrueOrFalse union = new False();
        switch (union) {
            case {IsTrue: true, AsTrue: var trueValue}:
                await Assert.That(trueValue).IsTypeOf<True>().And.IsEqualTo(new True());
                break;
            case {IsFalse: true, AsFalse: var falseValue}: 
                await Assert.That(falseValue).IsTypeOf<False>().And.IsEqualTo(new False());
                break;
        }
    }
}