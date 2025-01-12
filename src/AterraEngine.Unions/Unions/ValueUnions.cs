// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("T0")]
public readonly partial struct ValueUnion<T0>() 
    : IUnion<T0>
    where T0 : struct;

[UnionAliases("T0", "T1")]
public readonly partial struct ValueUnion<T0, T1>()
    : IUnion<T0, T1>
    where T0 : struct
    where T1 : struct;

[UnionAliases("T0", "T1", "T2")]
public readonly partial struct ValueUnion<T0, T1, T2>()
    : IUnion<T0, T1, T2>
    where T0 : struct
    where T1 : struct
    where T2 : struct;

[UnionAliases("T0", "T1", "T2", "T3")]
public readonly partial struct ValueUnion<T0, T1, T2, T3>()
    : IUnion<T0, T1, T2, T3>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4>()
    : IUnion<T0, T1, T2, T3, T4>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5>()
    : IUnion<T0, T1, T2, T3, T4, T5>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5, T6>()
    : IUnion<T0, T1, T2, T3, T4, T5, T6>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct
    where T6 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5, T6, T7>()
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct
    where T6 : struct
    where T7 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct
    where T6 : struct
    where T7 : struct
    where T8 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct
    where T6 : struct
    where T7 : struct
    where T8 : struct
    where T9 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct
    where T6 : struct
    where T7 : struct
    where T8 : struct
    where T9 : struct
    where T10 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct
    where T6 : struct
    where T7 : struct
    where T8 : struct
    where T9 : struct
    where T10 : struct
    where T11 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct
    where T6 : struct
    where T7 : struct
    where T8 : struct
    where T9 : struct
    where T10 : struct
    where T11 : struct
    where T12 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12", "T13")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct
    where T6 : struct
    where T7 : struct
    where T8 : struct
    where T9 : struct
    where T10 : struct
    where T11 : struct
    where T12 : struct
    where T13 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12", "T13", "T14")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct
    where T6 : struct
    where T7 : struct
    where T8 : struct
    where T9 : struct
    where T10 : struct
    where T11 : struct
    where T12 : struct
    where T13 : struct
    where T14 : struct;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12", "T13", "T14", "T15")]
public readonly partial struct ValueUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
    where T5 : struct
    where T6 : struct
    where T7 : struct
    where T8 : struct
    where T9 : struct
    where T10 : struct
    where T11 : struct
    where T12 : struct
    where T13 : struct
    where T14 : struct
    where T15 : struct;
