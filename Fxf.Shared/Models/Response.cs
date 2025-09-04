namespace Fxf.Shared.Models;

/// <summary>
/// ApiResponse class is a generic class that is used to return a response from the API.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>
{
	/// <summary>
	/// Indicates if the operation was successful.
	/// </summary>
	public bool Successful { get; set; } = true;

	/// <summary>
	/// Describes the response, can include success/failure message.
	/// </summary>
	public string? Message { get; set; }

	/// <summary>
	/// Optional data payload returned from the operation.
	/// </summary>
	public T? Data { get; set; }

	/// <summary>
	/// Creates a successful ApiResponse with optional message.
	/// </summary>
	public static ApiResponse<T> Success(T data, string? message = null) => new()
	{
		Successful = true,
		Data = data,
		Message = message
	};

	/// <summary>
	/// Creates a failed ApiResponse with message.
	/// </summary>
	public static ApiResponse<T> Fail(string message) => new()
	{
		Successful = false,
		Message = message,
		Data = default
	};
}