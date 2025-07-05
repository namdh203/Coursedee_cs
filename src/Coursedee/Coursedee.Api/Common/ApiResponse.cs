namespace Coursedee.Api.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> ResponseGeneral(bool success, string message, T? data)
    {
        return new ApiResponse<T>
        {
            Success = success,
            Message = message,
            Data = data
        };
    }
}
