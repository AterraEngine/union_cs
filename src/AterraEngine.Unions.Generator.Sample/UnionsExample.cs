// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions.Generator.Sample;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionGenerator<True, False>]
public readonly partial struct TrueOrFalse() {
    // public static implicit operator TrueOrFalse(bool value) => new() {
    //     Value = value,
    //     IsTrue = value,
    //     IsFalse = !value
    // };
}

// [UnionGenerator<(True, Success<string>), False>]
// public readonly partial struct TupleOrFalse() {
//     public static implicit operator TupleOrFalse(bool value) => new() {
//         Value = value,
//         IsTupleOfTrueAndSuccess = value,
//         IsFalse = !value
//     };
// }