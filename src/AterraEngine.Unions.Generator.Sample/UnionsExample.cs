// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;

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

public readonly partial struct TestWithDictionaries() : IUnion<List<string>, Dictionary<string,string>>;