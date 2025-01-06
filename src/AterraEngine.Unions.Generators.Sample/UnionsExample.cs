// // ---------------------------------------------------------------------------------------------------------------------
// // Imports
// // ---------------------------------------------------------------------------------------------------------------------
// using System.Collections.Generic;
// using System.Threading.Tasks;
//
// namespace AterraEngine.Unions.Generators.Sample;
// // ---------------------------------------------------------------------------------------------------------------------
// // Code
// // ---------------------------------------------------------------------------------------------------------------------
// public readonly partial record struct TrueOrFalse() : IUnion<True, False> {
//     public static implicit operator TrueOrFalse(bool value) => value ? new True() : new False();
// }
//
// public readonly partial struct TupleOrFalse() : IUnion<(True, Success<string>), False> {
//     public static implicit operator TupleOrFalse(bool value) {
//         if (value) return (new True(), new Success<string>(string.Empty));
//
//         return new False();
//     }
//     public static implicit operator TupleOrFalse(string value) => (new True(), new Success<string>(value));
// }
//
// public readonly partial struct TestWithArrays() : IUnion<List<string>, string[]>;
//
// public readonly partial struct TestWithDictionaries() : IUnion<List<string>, Dictionary<string, string>>;
//
// public class UnionExample {
//     public Union<string, int> GetSomeValue(bool input) {
//         if (input) return "Something";
//
//         return 0;
//     }
//
//     public async Task<bool> SomeAsyncAction() {
//         Union<string, int> union = GetSomeValue(true);
//         await union.SwitchAsync(
//             t0Case: async _ => await Task.Delay(100),
//             t1Case: async _ => await Task.Delay(100)
//         );
//
//         return true;
//     }
//
//     public TrueOrFalse MaybeGetSomething(int value) {
//         if (value == 0) return new True();
//
//         return new False();
//     }
//
//     public async Task<int> SomeAsyncActionWithUnion() {
//         TrueOrFalse union = MaybeGetSomething(0);
//         union.TryGetAsTrue(out True _);
//
//         return await union.MatchAsync(
//             trueCase: async _ => {
//                 await Task.Delay(100);
//                 return 1;
//             },
//             falseCase: async _ => {
//                 await Task.Delay(100);
//                 return 2;
//             }
//         );
//     }
// }
//
// // ---------------------------------------------------------------------------------------------------------------------
// // Aliases
// // ---------------------------------------------------------------------------------------------------------------------
// [UnionAliases(aliasT0: "Succeeded")]
// public readonly partial struct SucceededOrFalse() : IUnion<(Success<string>, None), False>;
//
// [UnionAliases(aliasT1: "Empty")]
// public readonly partial struct TupleOrEmpty() : IUnion<(True, Success<string>), False>;
//
// [UnionAliases("Nothing", "Something")]
// public readonly partial struct NothingOrSomething() : IUnion<True, False> {
//     public static implicit operator NothingOrSomething(bool value) => value ? new True() : new False();
// }
//
// [UnionAliases(aliasT2: "Alias")]
// public readonly partial struct TrueFalseOrAlias() : IUnion<True, False, None>;
//
// // ---------------------------------------------------------------------------------------------------------------------
// // Generics
// // ---------------------------------------------------------------------------------------------------------------------
// public partial struct GenericUnion<T>() : IUnion<Success<T>, None, Error<string>> {
//     public static implicit operator GenericUnion<T>(T value) => new Success<T>(value);
// }
//
// [UnionAliases(aliasT0: "SuccessWithValue")]
// public partial struct GenericUnionWithAlias<T>() : IUnion<Success<T>, None, Error<string>> {
//     public static implicit operator GenericUnionWithAlias<T>(T value) => new Success<T>(value);
// }

using AterraEngine.Unions;

namespace TestNamespace;
public readonly struct Success<T> {
    public T Value { get; init; }
}

public struct None;

public struct False;

[UnionExtra(UnionExtra.GenerateFrom)]
public readonly partial struct TupleOrFalse() : IUnion<(Success<string>, None), False> {}
