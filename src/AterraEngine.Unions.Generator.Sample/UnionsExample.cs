// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AterraEngine.Unions.Generator.Sample;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct TrueOrFalse() : IUnion<True, False> {
    public static implicit operator TrueOrFalse(bool value)  => value ? new True() : new False();
}

public readonly partial struct TupleOrFalse() : IUnion<(True, Success<string>), False> {
    public static implicit operator TupleOrFalse(bool value) {
        if (value) return (new True(), new Success<string>(string.Empty));
        return new False();
    }
    public static implicit operator TupleOrFalse(string value) => (new True(), new Success<string>(value));
}

public readonly partial struct TestWithArrays() : IUnion<List<string>, string[]>;

public readonly partial struct TestWithDictionaries() : IUnion<List<string>, Dictionary<string, string>> {
}


public class UnionExample {
    public Union<string, int> GetSomeValue(bool input) {
        if (input) return "Something";
        else return 0;
    }

    public async Task<bool> SomeAsyncAction() {
        var union = GetSomeValue(true);
        await union.SwitchAsync(
            async (string value) => await Task.Delay(100),
            async (int value) => await Task.Delay(100)
        );

        return true;
    }

    public TrueOrFalse MaybeGetSomething(int value) {
        if (value == 0) return new True();
        else return new False();
    }
    
    public async Task<int> SomeAsyncActionWithUnion() {
        TrueOrFalse union = MaybeGetSomething(0);
        union.TryGetAsTrue(out var value);
        
        return await union.MatchAsync(
            async @true => {
                await Task.Delay(100);
                return 1;
            },
            async @false => {
                await Task.Delay(100);
                return 2;
            }
        );
    }
}