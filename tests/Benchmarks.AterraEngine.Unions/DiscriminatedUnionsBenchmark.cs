// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CommandLine;
using Dunet;
using OneOf;
using OneOf.Types;
using None=AterraEngine.Unions.None;
using True=AterraEngine.Unions.True;

// To fix the warnings on the classes
// ReSharper disable InconsistentNaming

namespace Benchmarks.AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MemoryDiagnoser]  // Adds memory allocation info
[Orderer(SummaryOrderPolicy.FastestToSlowest)]  // Orders results by speed
public class DiscriminatedUnionsBenchmark {
    [Benchmark(Baseline = true)]  // Makes this the baseline for comparisons
    public True? AterraEngineUnions_TrueFalse_TryGetAsTrue() {
        global::AterraEngine.Unions.TrueOrFalse union = new True();
        if (union.TryGetAsTrue(out True result)) return result;
        return null;
    }

    [Benchmark]
    public global::AterraEngine.Unions.Success<string>? AterraEngineUnions_SuccessOrFailure_SwitchCase_Struct() {
        SuccessOrFailure<string, None> union = new global::AterraEngine.Unions.Success<string>("Something as success");
        switch (union) {
            case { IsSuccess: true, AsSuccess: var successValue }: return successValue;
            case { IsFailure: true, AsFailure: var failureValue }: return null;
            default: return default!;
        }
    }
    
    [Benchmark]
    public global::AterraEngine.Unions.Success<string>? AterraEngineUnions_SuccessOrFailure_SwitchCase_Value() {
        SuccessOrFailure<string, None> union = new global::AterraEngine.Unions.Success<string>("Something as success");
        switch (union.Value) {
            case global::AterraEngine.Unions.Success<string> success: return success;
            case Failure<None>: return null;
            default: return default!;
        }
    }

    [Benchmark]
    public string? AterraEngineUnions_UnionT8_SwitchCase_Value() {
        Union_T8 union = "value";
        switch (union.Value) {
            case bool : return null;
            case int : return null;
            case List<string> : return null;
            case float : return null;
            case double : return null;
            case short : return null;
            case Dictionary<int, bool>: return null;
            case string value : return value;
            
            default: return null;
        }
    }

    [Benchmark]
    public string? AterraEngineUnions_UnionT8_TryGetAs() {
        Union_T8 union = "value";
        if (union.TryGetAsString(out string result)) return result;
        return null;
    }

    // ---------------------------------------------------------------------------------------------------------------------
    // OneOf
    // ---------------------------------------------------------------------------------------------------------------------
    [Benchmark]
    public OneOf.Types.TrueOrFalse.True? OneOfTrueFalse_TryGetAsTrue() {
        OneOf.Types.TrueOrFalse union = new OneOf.Types.TrueOrFalse.True();
        if (union.TryPickT0(out OneOf.Types.TrueOrFalse.True result, out _)) return result;
        return null;
    }
    
    [Benchmark]
    public OneOf.Types.Success<string>? OneOf_SuccessOrFailure_SwitchCase_Value() {
        OneOf_SuccessOrFailure<string, string> union = new OneOf.Types.Success<string>();
        
        switch (union.Value) {
            case OneOf.Types.Success<string> successValue: return successValue;
            case OneOf_SuccessOrFailure<string,string>.Failure<string> : return null;
            default: return default!;
        }
    }
    
    [Benchmark]
    public string? OneOf_OneOfT8_SwitchCase_Value() {
        OneOf_T8 union = "value";
        switch (union.Value) {
            case bool : return null;
            case int : return null;
            case List<string> : return null;
            case float : return null;
            case double : return null;
            case short : return null;
            case Dictionary<int, bool> : return null;
            case string value : return value;
            
            default: return null;
        }
    }

    [Benchmark]
    public string? OneOf_OneOfT8_TryGetAs() {
        OneOf_T8 union = "value";
        if (union.TryPickT7(out string result, out _)) return result;
        return null;
    }

    // ---------------------------------------------------------------------------------------------------------------------
    // Dunet
    // ---------------------------------------------------------------------------------------------------------------------
    [Benchmark]
    public Dunet_TrueOrFalse.Dunet_True Dunet_TrueFalse_MatchTrue() {
        Dunet_TrueOrFalse union = new Dunet_TrueOrFalse.Dunet_True();
        Dunet_TrueOrFalse.Dunet_True? result = null;
        union.MatchDunet_True(
            @true => result = @true,
            () => result = null
        );
        return result!;
    }
}

// ---------------------------------------------------------------------------------------------------------------------
// Helper classes
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


