// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISuccessOrFailure<TSuccess> : ISuccessOrFailure<TSuccess, string>;
public interface ISuccessOrFailure<TSuccess, TFailure> :
    IIsSuccess, 
    IIsFailure,
    IAsSuccess<TSuccess>,
    IAsFailure<TFailure>,
    ITryGetAsSuccess<TSuccess>,
    ITryGetAsFailure<TFailure>,
    ITryGetAsSuccessValue<TSuccess>,
    ITryGetAsFailureValue<TFailure>,
    IMatch<TSuccess, TFailure>,
    IMatchAsync<TSuccess, TFailure>,
    ISwitch<TSuccess, TFailure>,
    ISwitchAsync<TSuccess, TFailure>;

public interface IIsSuccess {
    bool IsSuccess { get; }
}

public interface IIsFailure {
    bool IsFailure { get; }
}

public interface IAsSuccess<T> {
    Success<T> AsSuccess { get; }
}

public interface IAsFailure<T> {
    Failure<T> AsFailure { get; }
}

public interface ITryGetAsSuccess<T> {
    bool TryGetAsSuccess(out Success<T> value);
}

public interface ITryGetAsFailure<T> {
    bool TryGetAsFailure(out Failure<T> value);
}

public interface ITryGetAsSuccessValue<T> {
    bool TryGetAsSuccessValue([NotNullWhen(true)] out T? value);
}

public interface ITryGetAsFailureValue<T> {
    bool TryGetAsFailureValue([NotNullWhen(true)] out T? value);
}

public interface IMatch<TSuccess, TFailure> {
    TOutput Match<TOutput>(Func<Success<TSuccess>, TOutput> successCase, Func<Failure<TFailure>, TOutput> failureCase);
}

public interface IMatchAsync<TSuccess, TFailure> {
    Task<TOutput> MatchAsync<TOutput>(Func<Success<TSuccess>, Task<TOutput>> successCase, Func<Failure<TFailure>, Task<TOutput>> failureCase);
}

public interface ISwitch<TSuccess, TFailure> {
    void Switch(Action<Success<TSuccess>> successCase, Action<Failure<TFailure>> failureCase);
}

public interface ISwitchAsync<TSuccess, TFailure> {
    Task SwitchAsync(Func<Success<TSuccess>, Task> successCase, Func<Failure<TFailure>, Task> failureCase);
}
