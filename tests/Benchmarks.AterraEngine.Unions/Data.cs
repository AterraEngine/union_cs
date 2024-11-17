// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using Dunet;
using OneOf;

namespace Benchmarks.AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct Union_T8() : IUnion<
    bool,
    int,
    List<string>,
    float,
    double,
    short,
    Dictionary<int, bool>,
    string
>;

[GenerateOneOf]
public partial class OneOf_T8 : OneOfBase<
    bool,
    int,
    List<string>,
    float,
    double,
    short,
    Dictionary<int, bool>,
    string
>;


[GenerateOneOf]
public partial class OneOf_SuccessOrFailure<TSuccess, TFailure> : OneOfBase<OneOf.Types.Success<TSuccess>, OneOf_SuccessOrFailure<TSuccess, TFailure>.Failure<TFailure>> {
    public class Failure<T> { }
}

[Union]
public partial record Dunet_TrueOrFalse {
    public partial record Dunet_True;
    public partial record Dunet_False;
} 
