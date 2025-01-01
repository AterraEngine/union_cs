// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

// To fix the warnings on the classes
// ReSharper disable InconsistentNaming

namespace Benchmarks.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MemoryDiagnoser]// Adds memory allocation info
[Orderer(SummaryOrderPolicy.FastestToSlowest)]// Orders results by speed
public class DiscriminatedUnionsBenchmarkEnhanced {
    [Benchmark(OperationsPerInvoke = 1000)]
    public HashSet<string> AterraEngineUnions_UnionT8_TryGetAs_Enhanced() {
        var results = new HashSet<string>();
        for (int i = 0; i < 1000; i++) {
            Union_T8 union = "value";
            if (union.TryGetAsString(out string? result)) results.Add(result);
            results.Add(string.Empty);
        }

        return results;
    }

    [Benchmark(OperationsPerInvoke = 1000)]
    public HashSet<string> AterraEngineUnions_UnionT8_SwitchCase_Value_Enhanced() {
        var results = new HashSet<string>();
        for (int i = 0; i < 1000; i++) {
            Union_T8 union = "value";
            switch (union.Value) {
                case bool:
                case int:
                case List<string>:
                case float:
                case double:
                case short:
                case Dictionary<int, bool>:
                    results.Add(string.Empty);
                    break;

                case string value:
                    results.Add(value);
                    break;

                default:
                    results.Add(string.Empty);
                    break;
            }
        }

        return results;
    }

    // ---------------------------------------------------------------------------------------------------------------------
    // OneOf
    // ---------------------------------------------------------------------------------------------------------------------
    [Benchmark(OperationsPerInvoke = 1000)]
    public HashSet<string> OneOf_OneOfT8_TryGetAs_Enhanced() {
        var results = new HashSet<string>();
        for (int i = 0; i < 1000; i++) {
            OneOf_T8 union = "value";
            if (union.TryPickT7(out string result, out _)) results.Add(result);
            results.Add(string.Empty);
        }

        return results;
    }

    [Benchmark(OperationsPerInvoke = 1000)]
    public HashSet<string> OneOf_OneOfT8_SwitchCase_Value_Enhanced() {
        var results = new HashSet<string>();
        for (int i = 0; i < 1000; i++) {
            OneOf_T8 union = "value";
            switch (union.Value) {
                case bool:
                case int:
                case List<string>:
                case float:
                case double:
                case short:
                case Dictionary<int, bool>:
                    results.Add(string.Empty);
                    break;

                case string value:
                    results.Add(value);
                    break;

                default:
                    results.Add(string.Empty);
                    break;
            }
        }

        return results;
    }

    // ---------------------------------------------------------------------------------------------------------------------
    // Dunet
    // ---------------------------------------------------------------------------------------------------------------------
}
