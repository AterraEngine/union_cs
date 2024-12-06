// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct SuccessOrFailure<TSuccess, TFailure>() : IUnion<Success<TSuccess>, Failure<TFailure>> {
    public static implicit operator SuccessOrFailure<TSuccess, TFailure>(TSuccess value) => new Success<TSuccess>(value);
    public static implicit operator SuccessOrFailure<TSuccess, TFailure>(TFailure value) => new Failure<TFailure>(value);
    
    public bool TryGetAsSuccessValue([NotNullWhen(true)] out TSuccess? value) {
        value = default;
        if (!IsSuccess) return false;

        value = AsSuccess.Value;
        return value is not null;
    }
    
    public bool TryGetAsFailureValue([NotNullWhen(true)] out TFailure? value) {
        value = default;
        if (!IsFailure) return false;

        value = AsFailure.Value;
        return value is not null;
    }
}
