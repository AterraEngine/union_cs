// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using AterraEngine.Unions.Generator;
using JetBrains.Annotations;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Tests.AterraEngine.Unions.Generator;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionGeneratorTests : IncrementalGeneratorTest<UnionGenerator> {
    protected override Assembly[] ReferenceAssemblies { get; } = [
        typeof(object).Assembly,
        typeof(IUnion<>).Assembly,
        typeof(UnionAliasesAttribute).Assembly,
        typeof(ValueTuple).Assembly, // For tuples
        typeof(Attribute).Assembly,
        typeof(Console).Assembly,
        Assembly.Load("System.Runtime")
    ];

    // I hae no Clue why this is not working.
    // It is working in production, but in these tests, it just breaks
    // I might be something related to the IncrementalGeneratorTest<> configuration, but I have no clue.
    [Theory]
    [InlineData(TrueOrFalseInput, TrueOrFalseOutput)]
    [InlineData(TupleOrFalseInput, TupleOrFalseOutput)]
    [InlineData(SucceededOrFalseInput, SucceededOrFalseOutput)]
    [InlineData(NothingOrSomethingInput, NothingOrSomethingOutput)]
    [InlineData(TrueFalseOrAliasInput, TrueFalseOrAliasOutput)]
    public async Task TestText(string inputText, string expectedOutput) {
        await TestGeneratorAsync(inputText, expectedOutput, predicate: result => result.HintName.EndsWith("_Union.g.cs"));
    }

    #region Original Test
    [LanguageInjection("csharp")] private const string TrueOrFalseInput = """
        namespace TestNamespace {
            public struct True;
            public struct False;
            
            public readonly partial struct TrueOrFalse() : AterraEngine.Unions.IUnion<True, False> {
                public static implicit operator TrueOrFalse(bool value) => new() {
                    Value = value,
                    IsTrue = value,
                    IsFalse = !value
                };
            }
        }
        """;

    [LanguageInjection("csharp")] private const string TrueOrFalseOutput = """
        // <auto-generated />
        using System;
        using System.Diagnostics.CodeAnalysis;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct TrueOrFalse {
        
            #region True
            public bool IsTrue { get; init; } = false;
            public TestNamespace.True AsTrue {get; init;} = default!;
            public bool TryGetAsTrue(out TestNamespace.True value) {
                if (IsTrue) {
                    value = AsTrue;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TrueOrFalse(TestNamespace.True value) => new TrueOrFalse() {
                IsTrue = true,
                AsTrue = value
            };
            #endregion
        
            #region False
            public bool IsFalse { get; init; } = false;
            public TestNamespace.False AsFalse {get; init;} = default!;
            public bool TryGetAsFalse(out TestNamespace.False value) {
                if (IsFalse) {
                    value = AsFalse;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TrueOrFalse(TestNamespace.False value) => new TrueOrFalse() {
                IsFalse = true,
                AsFalse = value
            };
            #endregion
        
            public object? Value { get {
                if (IsTrue) return AsTrue;
                if (IsFalse) return AsFalse;
                throw new ArgumentOutOfRangeException();
            }}
        }
        """;
    #endregion

    #region Tuple Test
    [LanguageInjection("csharp")] private const string TupleOrFalseInput = """
        using System;
        namespace TestNamespace;

        public readonly struct Success<T> {
            public T Value { get; init; }
        }
        public struct None;
        public struct False;
                
        public readonly partial struct TupleOrFalse() : AterraEngine.Unions.IUnion<(Success<string>, None), False> { }
        """;

    [LanguageInjection("csharp")] private const string TupleOrFalseOutput = """
        // <auto-generated />
        using System;
        using System.Diagnostics.CodeAnalysis;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct TupleOrFalse {
        
            #region SuccessOfStringAndNoneTuple
            public bool IsSuccessOfStringAndNoneTuple { get; init; } = false;
            public (TestNamespace.Success<string>, TestNamespace.None) AsSuccessOfStringAndNoneTuple {get; init;} = default!;
            public bool TryGetAsSuccessOfStringAndNoneTuple(out (TestNamespace.Success<string>, TestNamespace.None) value) {
                if (IsSuccessOfStringAndNoneTuple) {
                    value = AsSuccessOfStringAndNoneTuple;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TupleOrFalse((TestNamespace.Success<string>, TestNamespace.None) value) => new TupleOrFalse() {
                IsSuccessOfStringAndNoneTuple = true,
                AsSuccessOfStringAndNoneTuple = value
            };
            #endregion
        
            #region False
            public bool IsFalse { get; init; } = false;
            public TestNamespace.False AsFalse {get; init;} = default!;
            public bool TryGetAsFalse(out TestNamespace.False value) {
                if (IsFalse) {
                    value = AsFalse;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TupleOrFalse(TestNamespace.False value) => new TupleOrFalse() {
                IsFalse = true,
                AsFalse = value
            };
            #endregion
        
            public object? Value { get {
                if (IsSuccessOfStringAndNoneTuple) return AsSuccessOfStringAndNoneTuple;
                if (IsFalse) return AsFalse;
                throw new ArgumentOutOfRangeException();
            }}
        }
        
        """;
    #endregion

    #region SucceededOrFalse Test (Alias)
    [LanguageInjection("csharp")] private const string SucceededOrFalseInput = """
        namespace TestNamespace;

        public readonly struct Success<T> {
            public T Value { get; init; }
        }
        public struct None;
        public struct False;

        [AterraEngine.Unions.UnionAliases(aliasT0:"Succeeded")]
        public readonly partial struct SucceededOrFalse() : AterraEngine.Unions.IUnion<(Success<string>, None), False> { }
        """;

    [LanguageInjection("csharp")] private const string SucceededOrFalseOutput = """
        // <auto-generated />
        using System;
        using System.Diagnostics.CodeAnalysis;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct SucceededOrFalse {
        
            #region Succeeded
            public bool IsSucceeded { get; init; } = false;
            public (TestNamespace.Success<string>, TestNamespace.None) AsSucceeded {get; init;} = default!;
            public bool TryGetAsSucceeded(out (TestNamespace.Success<string>, TestNamespace.None) value) {
                if (IsSucceeded) {
                    value = AsSucceeded;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator SucceededOrFalse((TestNamespace.Success<string>, TestNamespace.None) value) => new SucceededOrFalse() {
                IsSucceeded = true,
                AsSucceeded = value
            };
            #endregion
        
            #region False
            public bool IsFalse { get; init; } = false;
            public TestNamespace.False AsFalse {get; init;} = default!;
            public bool TryGetAsFalse(out TestNamespace.False value) {
                if (IsFalse) {
                    value = AsFalse;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator SucceededOrFalse(TestNamespace.False value) => new SucceededOrFalse() {
                IsFalse = true,
                AsFalse = value
            };
            #endregion
        
            public object? Value { get {
                if (IsSucceeded) return AsSucceeded;
                if (IsFalse) return AsFalse;
                throw new ArgumentOutOfRangeException();
            }}
        }
        """;
    #endregion
    
    #region Alias All Test
    [LanguageInjection("csharp")] private const string NothingOrSomethingInput = """
        namespace TestNamespace;

        public struct True;
        public struct False;

        [AterraEngine.Unions.UnionAliases("Nothing", "Something")]
        public readonly partial struct NothingOrSomething() : AterraEngine.Unions.IUnion<True, False> { }
        """;

    [LanguageInjection("csharp")] private const string NothingOrSomethingOutput = """
        // <auto-generated />
        using System;
        using System.Diagnostics.CodeAnalysis;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct NothingOrSomething {
        
            #region Nothing
            public bool IsNothing { get; init; } = false;
            public TestNamespace.True AsNothing {get; init;} = default!;
            public bool TryGetAsNothing(out TestNamespace.True value) {
                if (IsNothing) {
                    value = AsNothing;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator NothingOrSomething(TestNamespace.True value) => new NothingOrSomething() {
                IsNothing = true,
                AsNothing = value
            };
            #endregion
        
            #region Something
            public bool IsSomething { get; init; } = false;
            public TestNamespace.False AsSomething {get; init;} = default!;
            public bool TryGetAsSomething(out TestNamespace.False value) {
                if (IsSomething) {
                    value = AsSomething;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator NothingOrSomething(TestNamespace.False value) => new NothingOrSomething() {
                IsSomething = true,
                AsSomething = value
            };
            #endregion
        
            public object? Value { get {
                if (IsNothing) return AsNothing;
                if (IsSomething) return AsSomething;
                throw new ArgumentOutOfRangeException();
            }}
        }
        """;
    #endregion
    
    #region Alias Skip Test
    [LanguageInjection("csharp")] private const string TrueFalseOrAliasInput = """
        namespace TestNamespace;

        public struct True;
        public struct False;
        public struct Done;

        [AterraEngine.Unions.UnionAliases(aliasT2: "Alias")]
        public readonly partial struct TrueFalseOrAlias() : AterraEngine.Unions.IUnion<True, False, Done> { }
        """;

    [LanguageInjection("csharp")] private const string TrueFalseOrAliasOutput = """
        // <auto-generated />
        using System;
        using System.Diagnostics.CodeAnalysis;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct TrueFalseOrAlias {
        
            #region True
            public bool IsTrue { get; init; } = false;
            public TestNamespace.True AsTrue {get; init;} = default!;
            public bool TryGetAsTrue(out TestNamespace.True value) {
                if (IsTrue) {
                    value = AsTrue;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TrueFalseOrAlias(TestNamespace.True value) => new TrueFalseOrAlias() {
                IsTrue = true,
                AsTrue = value
            };
            #endregion
        
            #region False
            public bool IsFalse { get; init; } = false;
            public TestNamespace.False AsFalse {get; init;} = default!;
            public bool TryGetAsFalse(out TestNamespace.False value) {
                if (IsFalse) {
                    value = AsFalse;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TrueFalseOrAlias(TestNamespace.False value) => new TrueFalseOrAlias() {
                IsFalse = true,
                AsFalse = value
            };
            #endregion
        
            #region Alias
            public bool IsAlias { get; init; } = false;
            public TestNamespace.Done AsAlias {get; init;} = default!;
            public bool TryGetAsAlias(out TestNamespace.Done value) {
                if (IsAlias) {
                    value = AsAlias;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TrueFalseOrAlias(TestNamespace.Done value) => new TrueFalseOrAlias() {
                IsAlias = true,
                AsAlias = value
            };
            #endregion
        
            public object? Value { get {
                if (IsTrue) return AsTrue;
                if (IsFalse) return AsFalse;
                if (IsAlias) return AsAlias;
                throw new ArgumentOutOfRangeException();
            }}
        }
        """;
    #endregion
}