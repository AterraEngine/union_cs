// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Struct)]
public class UnionGeneratorAttribute(params Type[] types) : Attribute {
    public Type[] Types { get; } = types;
}

public class UnionGeneratorAttribute<T0>() : UnionGeneratorAttribute(typeof(T0));
public class UnionGeneratorAttribute<T0, T1>() : UnionGeneratorAttribute(typeof(T0), typeof(T1));
public class UnionGeneratorAttribute<T0, T1, T2>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2));
public class UnionGeneratorAttribute<T0, T1, T2, T3>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5, T6>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5, T6, T7>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5, T6, T7, T8>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));
public class UnionGeneratorAttribute<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>() : UnionGeneratorAttribute(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));
