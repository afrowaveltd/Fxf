namespace Fxf.Shared.Models;

/// <summary>
/// Represents a standardized response that contains data and metadata about the operation.
/// </summary>
/// <typeparam name="T">The type of the returned data.</typeparam>
public class Response<T>
{
	/// <summary>
	/// Gets or sets the data returned by the operation.
	/// </summary>
	public T? Data { get; set; }

	/// <summary>
	/// Gets or sets the execution time of the operation, in milliseconds.
	/// </summary>
	public int ExecutionTime { get; set; } = 0;

	/// <summary>
	/// Gets or sets the message associated with the response.
	/// </summary>
	public string? Message { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the operation was successful.
	/// </summary>
	public bool Success { get; set; } = true;

	/// <summary>
	/// Gets or sets a value indicating whether the response contains a warning.
	/// </summary>
	public bool Warning { get; set; } = false;

	/// <summary>
	/// Creates a failed response with a message.
	/// </summary>
	/// <param name="message">The failure message.</param>
	/// <returns>A failed <see cref="Response{T}"/> instance.</returns>
	public static Response<T> Fail(string message)
	{
		return new Response<T>
		{
			Success = false,
			Message = message ?? string.Empty
		};
	}

	/// <summary>
	/// Creates a failed response with the specified error message and execution time.
	/// </summary>
	/// <param name="message">
	/// The error message describing the failure. If null, an empty string is used.
	/// </param>
	/// <param name="executionMs">The time, in milliseconds, that the operation took to execute.</param>
	/// <returns>A <see cref="Response{T}"/> instance representing a failed operation.</returns>
	public static Response<T> Fail(string message, int executionMs)
	{
		return new Response<T>
		{
			Success = false,
			Message = message ?? string.Empty,
			ExecutionTime = executionMs
		};
	}

	/// <summary>
	/// Creates a failed response from an exception.
	/// </summary>
	/// <param name="ex">The exception that caused the failure.</param>
	/// <returns>A failed <see cref="Response{T}"/> instance.</returns>
	public static Response<T> Fail(Exception ex)
	{
		return new Response<T>
		{
			Success = false,
			Message = ex?.Message ?? "Unknown error"
		};
	}

	/// <summary>
	/// Creates a failed response with the specified exception and execution time.
	/// </summary>
	/// <param name="ex">
	/// The exception that caused the failure. If null, a default message of "Unknown error" is used.
	/// </param>
	/// <param name="executionMs">The time, in milliseconds, that the operation took to execute.</param>
	/// <returns>
	/// A <see cref="Response{T}"/> object representing a failed operation, including the error
	/// message and execution time.
	/// </returns>
	public static Response<T> Fail(Exception ex, int executionMs)
	{
		return new Response<T>
		{
			Success = false,
			Message = ex?.Message ?? "Unknown error",
			ExecutionTime = executionMs
		};
	}

	/// <summary>
	/// Creates a failed response with the specified error message and exception details.
	/// </summary>
	/// <param name="message">The error message describing the failure.</param>
	/// <param name="ex">The exception that caused the failure.</param>
	/// <returns>
	/// A <see cref="Response{T}"/> instance representing a failed operation, containing the combined
	/// error message.
	/// </returns>
	public static Response<T> Fail(string message, Exception ex)
	{
		return new Response<T>
		{
			Success = false,
			Message = message + "" + ex.Message
		};
	}

	/// <summary>
	/// Creates a failed response with the specified error message, exception details, and execution time.
	/// </summary>
	/// <param name="message">The error message describing the failure.</param>
	/// <param name="ex">The exception that caused the failure. Cannot be null.</param>
	/// <param name="executionMs">The time, in milliseconds, that the operation took to execute.</param>
	/// <returns>A <see cref="Response{T}"/> object representing a failed operation.</returns>
	public static Response<T> Fail(string message, Exception ex, int executionMs)
	{
		return new Response<T>
		{
			Success = false,
			Message = message + "" + ex.Message,
			ExecutionTime = executionMs
		};
	}

	/// <summary>
	/// Creates a successful response with data and a message.
	/// </summary>
	/// <param name="data">The data to return.</param>
	/// <param name="message">The success message.</param>
	/// <param name="executionMs">The running time in milliseconds</param>
	/// <returns>A successful <see cref="Response{T}"/> instance.</returns>
	public static Response<T> Successful(T data, string message, int executionMs = 0)
	{
		return new Response<T>
		{
			Success = true,
			Data = data,
			Message = message ?? string.Empty,
			ExecutionTime = executionMs
		};
	}

	/// <summary>
	/// Creates a successful response with the specified data and message.
	/// </summary>
	/// <param name="data">
	/// The data to include in the response. This represents the result of the operation.
	/// </param>
	/// <param name="message">
	/// An optional message providing additional context about the success. If null, an empty string
	/// is used.
	/// </param>
	/// <returns>
	/// A <see cref="Response{T}"/> object representing a successful operation, containing the
	/// provided data and message.
	/// </returns>
	public static Response<T> Successful(T data, string message)
	{
		return new Response<T>
		{
			Success = true,
			Data = data,
			Message = message ?? string.Empty,
			ExecutionTime = 0
		};
	}

	/// <summary>
	/// Creates a successful response containing the specified data.
	/// </summary>
	/// <param name="data">
	/// The data to include in the response. This value represents the result of the operation.
	/// </param>
	/// <returns>
	/// A <see cref="Response{T}"/> object with <see cref="Response{T}.Success"/> set to <see
	/// langword="true"/>, the specified <paramref name="data"/> assigned to <see
	/// cref="Response{T}.Data"/>, and an empty message.
	/// </returns>
	public static Response<T> Successful(T data)
	{
		return new Response<T>
		{
			Success = true,
			Data = data,
			Message = string.Empty
		};
	}

	/// <summary>
	/// Creates a successful response containing the specified data and execution time.
	/// </summary>
	/// <param name="data">The data to include in the response.</param>
	/// <param name="executionMs">The time taken to execute the operation, in milliseconds.</param>
	/// <returns>
	/// A <see cref="Response{T}"/> object representing a successful operation, with the specified
	/// data and execution time.
	/// </returns>
	public static Response<T> Successful(T data, int executionMs)
	{
		return new Response<T>
		{
			Success = true,
			Data = data,
			Message = string.Empty,
			ExecutionTime = executionMs
		};
	}

	/// <summary>
	/// Creates a successful response with no data and no message.
	/// </summary>
	/// <returns>A successful <see cref="Response{T}"/> instance.</returns>
	public static Response<T> Successful()
	{
		return new Response<T>
		{
			Success = true
		};
	}

	/// <summary>
	/// Creates a successful response with a warning, data, and a warning message.
	/// </summary>
	/// <param name="data">The data to return.</param>
	/// <param name="warningMessage">The warning message.</param>
	/// <returns>A successful <see cref="Response{T}"/> instance with a warning.</returns>
	public static Response<T> SuccessWithWarning(T data, string warningMessage)
	{
		return new Response<T>
		{
			Success = true,
			Warning = true,
			Data = data,
			Message = warningMessage ?? string.Empty
		};
	}
}