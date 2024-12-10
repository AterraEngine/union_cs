// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a generic interface to encapsulate a result that can either be a success or a failure.
/// </summary>
/// <typeparam name="TSuccess">
///     The type of the value when the result represents a success.
/// </typeparam>
public interface ISuccessOrFailure<TSuccess> : ISuccessOrFailure<TSuccess, string>;

/// <summary>
///     Represents a result type that can be either a success or failure, encapsulating transformations,
///     state-checking, and retrieval mechanisms for the respective case.
/// </summary>
/// <typeparam name="TSuccess">The type for the success case.</typeparam>
/// <typeparam name="TFailure">The type for the failure case.</typeparam>
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

/// <summary>
///     Represents an interface that indicates whether an operation succeeded.
///     Provides an abstraction for determining success state using a boolean property `IsSuccess`
///     without exposing details of the success or failure content.
/// </summary>
public interface IIsSuccess {
    /// <summary>
    ///     Indicates whether the operation represented by the current instance was successful.
    ///     A value of `true` means the operation succeeded, while a value of `false` indicates failure.
    /// </summary>
    bool IsSuccess { get; }
}

/// <summary>
///     Represents functionality for determining if an operation or result is a failure.
/// </summary>
public interface IIsFailure {
    /// <summary>
    ///     Indicates whether the current state represents a failure result.
    ///     This property returns a boolean value, where true signifies a failure state
    ///     and false signifies a success state.
    /// </summary>
    bool IsFailure { get; }
}

/// <summary>
///     Represents the contract for retrieving a successful value encapsulated within an operation's result.
/// </summary>
/// <typeparam name="T">The type of the success value.</typeparam>
public interface IAsSuccess<T> {
    /// <summary>
    ///     Gets the successful outcome as a <see cref="Success{T}" /> instance.
    ///     This property is used to represent and access a successful state or value within the union type.
    /// </summary>
    Success<T> AsSuccess { get; }
}

/// <summary>
///     Defines the functionality to retrieve a failure value in a strongly-typed union structure.
/// </summary>
/// <typeparam name="T">The type of the failure value.</typeparam>
public interface IAsFailure<T> {
    /// <summary>
    ///     Provides access to the failure state of the implementing type as a <see cref="Failure{T}" /> instance.
    /// </summary>
    /// <remarks>
    ///     This property is used to retrieve the failure value encapsulated within an implementation of the
    ///     <see cref="IAsFailure{T}" /> interface. It represents an unsuccessful state and contains the associated
    ///     failure value of type <typeparamref name="T" />.
    /// </remarks>
    Failure<T> AsFailure { get; }
}

/// <summary>
///     Provides functionality to attempt retrieval of a value as a successful result of type <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The type of the successful result.</typeparam>
public interface ITryGetAsSuccess<T> {
    /// <summary>
    ///     Attempts to retrieve the value as a Success type.
    /// </summary>
    /// <param name="value">
    ///     When this method returns, contains the Success object if the operation was successful; otherwise,
    ///     contains the default value.
    /// </param>
    /// <returns>Returns true if the value could be retrieved as a Success; otherwise, returns false.</returns>
    bool TryGetAsSuccess(out Success<T> value);
}

/// <summary>
///     Represents a type that attempts to retrieve a failure result from a union of success and failure outcomes.
/// </summary>
/// <typeparam name="T">The type of the failure value.</typeparam>
public interface ITryGetAsFailure<T> {
    /// <summary>
    ///     Attempts to retrieve the current instance as a failure value.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the failure.</typeparam>
    /// <param name="value">
    ///     When this method returns, contains the failure value, if the operation was successful; otherwise,
    ///     the default value for the type of the parameter.
    /// </param>
    /// <returns>
    ///     true if the operation was successful and the current instance represents a failure; otherwise, false.
    /// </returns>
    bool TryGetAsFailure(out Failure<T> value);
}

/// <summary>
///     Defines an operation for conditionally retrieving the success value from an object,
///     if it is in a success state.
/// </summary>
/// <typeparam name="T">The type of the success value.</typeparam>
public interface ITryGetAsSuccessValue<T> {
    /// <summary>
    ///     Attempts to retrieve the value of the success case from the current instance if it represents a success.
    /// </summary>
    /// <param name="value">
    ///     When this method returns, contains the success value of the current instance if it represents a success, or the
    ///     default value of <typeparamref name="T" /> if it does not.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the current instance represents a success and the success value is not null; otherwise, <c>false</c>
    ///     .
    /// </returns>
    bool TryGetAsSuccessValue([NotNullWhen(true)] out T? value);
}

/// <summary>
///     Provides functionality to attempt retrieval of failure values from an underlying union type.
/// </summary>
/// <typeparam name="T">The type of the failure value.</typeparam>
public interface ITryGetAsFailureValue<T> {
    /// <summary>
    ///     Attempts to retrieve the value of the failure case if the instance represents a failure.
    /// </summary>
    /// <param name="value">
    ///     The output parameter that will contain the failure value if the instance represents a failure;
    ///     otherwise, it will be set to its default value.
    /// </param>
    /// <returns>True if the instance represents a failure and outputs the failure value; otherwise, false.</returns>
    bool TryGetAsFailureValue([NotNullWhen(true)] out T? value);
}

/// <summary>
///     Defines an interface for implementing pattern matching functionality
///     for success and failure states within a union type.
/// </summary>
/// <typeparam name="TSuccess">
///     Specifies the type of the success value in the union type.
/// </typeparam>
/// <typeparam name="TFailure">
///     Specifies the type of the failure value in the union type.
/// </typeparam>
public interface IMatch<TSuccess, TFailure> {
    /// <summary>
    ///     Matches the current state of the union and executes the appropriate delegate
    ///     based on whether it represents a success or failure state.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output returned by the matched delegate.</typeparam>
    /// <param name="successCase">
    ///     A delegate that is executed if the current state represents a success,
    ///     receiving an instance of Success&lt;TSuccess&gt;.
    /// </param>
    /// <param name="failureCase">
    ///     A delegate that is executed if the current state represents a failure,
    ///     receiving an instance of Failure&lt;TFailure&gt;.
    /// </param>
    /// <returns>The result of executing either the success or failure delegate.</returns>
    TOutput Match<TOutput>(Func<Success<TSuccess>, TOutput> successCase, Func<Failure<TFailure>, TOutput> failureCase);
}

/// <summary>
///     Defines an asynchronous mechanism for matching between Success and Failure cases.
/// </summary>
/// <typeparam name="TSuccess">The type of the value in the Success case.</typeparam>
/// <typeparam name="TFailure">The type of the value in the Failure case.</typeparam>
public interface IMatchAsync<TSuccess, TFailure> {
    /// <summary>
    ///     Asynchronously matches the current instance against one of two provided cases, invoking the appropriate function
    ///     based on the instance type.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output returned by the match operation.</typeparam>
    /// <param name="successCase">A function to handle the case when the current instance is of type Success.</param>
    /// <param name="failureCase">A function to handle the case when the current instance is of type Failure.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the result of invoking the appropriate case
    ///     function.
    /// </returns>
    Task<TOutput> MatchAsync<TOutput>(Func<Success<TSuccess>, Task<TOutput>> successCase, Func<Failure<TFailure>, Task<TOutput>> failureCase);
}

/// <summary>
///     Represents a switch mechanism to handle both success and failure cases.
/// </summary>
/// <typeparam name="TSuccess">The type representing the success case.</typeparam>
/// <typeparam name="TFailure">The type representing the failure case.</typeparam>
public interface ISwitch<TSuccess, TFailure> {
    /// <summary>
    ///     Executes the provided actions based on whether the instance represents a success or failure.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    /// <typeparam name="TFailure">The type of the failure value.</typeparam>
    /// <param name="successCase">The action to execute if the instance represents a success.</param>
    /// <param name="failureCase">The action to execute if the instance represents a failure.</param>
    void Switch(Action<Success<TSuccess>> successCase, Action<Failure<TFailure>> failureCase);
}

/// <summary>
///     Defines an asynchronous switch operation for handling the two cases: success and failure.
/// </summary>
/// <typeparam name="TSuccess">The type of the success case value.</typeparam>
/// <typeparam name="TFailure">The type of the failure case value.</typeparam>
public interface ISwitchAsync<TSuccess, TFailure> {
    /// <summary>
    ///     Executes one of the given asynchronous functions depending on whether the instance represents a success or a
    ///     failure.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    /// <typeparam name="TFailure">The type of the failure value.</typeparam>
    /// <param name="successCase">The function to execute if the instance represents a success value.</param>
    /// <param name="failureCase">The function to execute if the instance represents a failure value.</param>
    /// <returns>A task that represents the asynchronous operation of executing the appropriate function.</returns>
    Task SwitchAsync(Func<Success<TSuccess>, Task> successCase, Func<Failure<TFailure>, Task> failureCase);
}
