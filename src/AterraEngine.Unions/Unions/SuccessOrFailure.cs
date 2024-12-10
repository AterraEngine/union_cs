// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a union type that encapsulates either a success value of type <typeparamref name="TSuccess"/>
/// or a failure value of type <typeparamref name="TFailure"/>.
/// </summary>
/// <typeparam name="TSuccess">The type of the success value.</typeparam>
/// <typeparam name="TFailure">The type of the failure value.</typeparam>
/// <remarks>
/// This struct provides functionality to easily work with operations that can result in either success or failure.
/// Implicit conversions are provided to create a success instance from a value of type <typeparamref name="TSuccess"/>
/// or a failure instance from a value of type <typeparamref name="TFailure"/>.
/// The struct also supports checking whether the state represents success or failure, and retrieving the corresponding value.
/// </remarks>
public readonly partial struct SuccessOrFailure<TSuccess, TFailure>() : IUnion<Success<TSuccess>, Failure<TFailure>>, ISuccessOrFailure<TSuccess, TFailure> {
    /// <summary>
    /// Defines implicit operators for the <see cref="SuccessOrFailure{TSuccess, TFailure}"/> struct,
    /// enabling conversions between success, failure, and boolean representations.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    /// <typeparam name="TFailure">The type of the failure value.</typeparam>
    /// <remarks>
    /// The implicit conversion operators allow seamless transition between:
    /// - A success value of type <typeparamref name="TSuccess"/> to <see cref="SuccessOrFailure{TSuccess, TFailure}"/>.
    /// - A failure value of type <typeparamref name="TFailure"/> to <see cref="SuccessOrFailure{TSuccess, TFailure}"/>.
    /// - A <see cref="SuccessOrFailure{TSuccess, TFailure}"/> instance to a boolean that indicates success.
    /// </remarks>
    public static implicit operator SuccessOrFailure<TSuccess, TFailure>(TSuccess value) => new Success<TSuccess>(value);
    /// <summary>
    /// Defines an implicit operator for the <see cref="SuccessOrFailure{TSuccess, TFailure}"/> struct,
    /// enabling conversion from a failure value of type <typeparamref name="TFailure"/> to
    /// <see cref="SuccessOrFailure{TSuccess, TFailure}"/>.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    /// <typeparam name="TFailure">The type of the failure value.</typeparam>
    /// <remarks>
    /// This implicit conversion operator allows effortless creation of a failure instance of
    /// <see cref="SuccessOrFailure{TSuccess, TFailure}"/> using a value of type <typeparamref name="TFailure"/>.
    /// The resulting instance can be used in scenarios where either success or failure is represented in a single structure.
    /// </remarks>
    public static implicit operator SuccessOrFailure<TSuccess, TFailure>(TFailure value) => new Failure<TFailure>(value);
    /// <summary>
    /// Defines an implicit conversion operator to boolean for the <see cref="SuccessOrFailure{TSuccess, TFailure}"/> type,
    /// indicating whether the current instance represents a success state.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    /// <typeparam name="TFailure">The type of the failure value.</typeparam>
    /// <param name="successOrFailure">The <see cref="SuccessOrFailure{TSuccess, TFailure}"/> instance to be evaluated.</param>
    /// <returns>
    /// A boolean value: true if the instance is in a success state, false otherwise.
    /// </returns>
    /// <remarks>
    /// This operator enables usage of <see cref="SuccessOrFailure{TSuccess, TFailure}"/> as a boolean condition
    /// in conditional checks. A successful state evaluates to true, while a failure state evaluates to false.
    /// </remarks>
    public static implicit operator bool(SuccessOrFailure<TSuccess, TFailure> successOrFailure) => successOrFailure.IsSuccess;

    /// <summary>
    /// Attempts to retrieve the value stored in the success case of the union, if it is a success.
    /// </summary>
    /// <param name="value">When this method returns, contains the success value if the union represents a success; otherwise, the default value of <typeparamref name="TSuccess"/>.</param>
    /// <returns>True if the union is a success and contains a valid success value; otherwise, false.</returns>
    public bool TryGetAsSuccessValue([NotNullWhen(true)] out TSuccess? value) {
        value = default;
        if (!IsSuccess) return false;

        value = AsSuccess.Value;
        return value is not null;
    }

    /// <summary>
    /// Attempts to retrieve the value of the instance as a failure value if it represents a failure.
    /// </summary>
    /// <param name="value">When this method returns, contains the failure value if the instance represents a failure; otherwise, the default value of <typeparamref name="TFailure"/>.</param>
    /// <returns>true if the instance represents a failure; otherwise, false.</returns>
    public bool TryGetAsFailureValue([NotNullWhen(true)] out TFailure? value) {
        value = default;
        if (!IsFailure) return false;

        value = AsFailure.Value;
        return value is not null;
    }
}

/// <summary>
/// Represents a discriminated union type which can either be a success or a failure.
/// </summary>
/// <typeparam name="TSuccess">The type of the success value.</typeparam>
[UnionAliases(null, "Failure")]
public readonly partial struct SuccessOrFailure<TSuccess>() : IUnion<Success<TSuccess>, Failure<string>>, ISuccessOrFailure<TSuccess> {
    /// <summary>
    /// Defines an implicit conversion from a value of type <typeparamref name="TSuccess"/> to a <see cref="SuccessOrFailure{TSuccess}"/> instance,
    /// creating a new <see cref="Success{TSuccess}"/> object with the provided value.
    /// </summary>
    /// <param name="value">The success value to be wrapped in a <see cref="Success{TSuccess}"/>.</param>
    /// <returns>A <see cref="SuccessOrFailure{TSuccess}"/> containing the success value.</returns>
    public static implicit operator SuccessOrFailure<TSuccess>(TSuccess value) => new Success<TSuccess>(value);
    /// <summary>
    /// Defines an implicit operator for converting a string into an instance of the <see cref="SuccessOrFailure{TSuccess}"/> struct.
    /// </summary>
    /// <typeparam name="TSuccess">The type parameter for the success value of the structure.</typeparam>
    /// <remarks>
    /// This operator allows implicit conversion of a string into an instance of <see cref="SuccessOrFailure{TSuccess}"/>
    /// by interpreting the string as a failure value. The resulting instance represents a failure state.
    /// </remarks>
    public static implicit operator SuccessOrFailure<TSuccess>(string value) => new Failure<string>(value);
    /// <summary>
    /// Defines an implicit conversion to a boolean for a <see cref="SuccessOrFailure{TSuccess}"/> instance.
    /// </summary>
    /// <param name="successOrFailure">
    /// An instance of the <see cref="SuccessOrFailure{TSuccess}"/> union, representing either a successful outcome
    /// or a failure outcome.
    /// </param>
    /// <returns>
    /// Returns <c>true</c> if the instance represents a success; otherwise, returns <c>false</c>.
    /// </returns>
    public static implicit operator bool(SuccessOrFailure<TSuccess> successOrFailure) => successOrFailure.IsSuccess;

    /// <summary>
    /// Attempts to extract the success value from the current instance if it represents a success case.
    /// </summary>
    /// <param name="value">
    /// An output parameter that will hold the success value if the current instance represents a success case; otherwise, it will be set to its default value.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the current instance represents a success case.
    /// </returns>
    public bool TryGetAsSuccessValue([NotNullWhen(true)] out TSuccess? value) {
        value = default;
        if (!IsSuccess) return false;

        value = AsSuccess.Value;
        return value is not null;
    }

    /// <summary>
    /// Attempts to extract the failure value from the current instance.
    /// </summary>
    /// <param name="value">If the operation succeeds, this will contain the failure value; otherwise, it will be null.</param>
    /// <returns>
    /// True if the current instance represents a failure and the failure value was extracted; otherwise, false.
    /// </returns>
    public bool TryGetAsFailureValue([NotNullWhen(true)] out string? value) {
        value = default;
        if (!IsFailure) return false;

        value = AsFailure.Value;
        return true;
    }
}

/// <summary>
/// Represents a union type that encapsulates either a success state or a failure state.
/// </summary>
/// <remarks>
/// This type is a discriminated union designed to handle cases where either a successful result or a failure result
/// can occur. It combines the semantics of <c>Success</c> and <c>Failure</c>, providing a robust way to express
/// two mutually exclusive outcomes.
/// </remarks>
[UnionAliases(null, "Failure")]
public readonly partial struct SuccessOrFailure() : IUnion<Success, Failure<string>> {
    /// <summary>
    /// Defines an implicit conversion operator that converts a <see cref="string"/> to a <see cref="SuccessOrFailure"/> instance of type <see cref="Failure{T}"/>.
    /// </summary>
    /// <param name="value">
    /// The string value to be used in the creation of a new <see cref="Failure{T}"/>.
    /// </param>
    /// <returns>
    /// A <see cref="SuccessOrFailure"/> instance containing a <see cref="Failure{T}"/> with the provided string as its value.
    /// </returns>
    public static implicit operator SuccessOrFailure(string value) => new Failure<string>(value);
    /// <summary>
    /// Defines an implicit conversion operator for the <see cref="SuccessOrFailure{TSuccess, TFailure}"/> struct,
    /// enabling seamless conversion to a boolean value representing the success state.
    /// </summary>
    /// <remarks>
    /// This operator allows a <see cref="SuccessOrFailure{TSuccess, TFailure}"/> instance to be evaluated directly
    /// in a boolean context, where <c>true</c> indicates a successful state and <c>false</c> signifies a failure state.
    /// </remarks>
    public static implicit operator bool(SuccessOrFailure successOrFailure) => successOrFailure.IsSuccess;

    /// <summary>
    /// Attempts to retrieve the failure value from the current instance if it represents a failure.
    /// </summary>
    /// <param name="value">When the method returns, contains the failure value if the instance is a failure; otherwise, null.</param>
    /// <returns>True if the current instance represents a failure and the failure value was successfully retrieved; otherwise, false.</returns>
    public bool TryGetAsFailureValue([NotNullWhen(true)] out string? value) {
        value = default;
        if (!IsFailure) return false;

        value = AsFailure.Value;
        return true;
    }
}