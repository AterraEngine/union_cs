// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct SuccessOrFailure<TSuccess, TFailure>() : IUnion<Success<TSuccess>, Failure<TFailure>> {
    public static implicit operator SuccessOrFailure<TSuccess, TFailure>(TSuccess value) => new Success<TSuccess>(value);
    public static implicit operator SuccessOrFailure<TSuccess, TFailure>(TFailure value) => new Failure<TFailure>(value);
}
