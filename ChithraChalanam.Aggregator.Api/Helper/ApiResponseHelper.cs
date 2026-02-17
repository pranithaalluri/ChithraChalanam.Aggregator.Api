using ChithraChalanam.Aggregator.Api.Common;

namespace ChithraChalanam.Aggregator.Api.Helper;

public static class ApiResponseHelper
{
    public static ApiResponse<T> Success<T>(T data, string message, int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Fail<T>(string message, int statusCode)
    {
        return new ApiResponse<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Data = default
        };
    }
}

