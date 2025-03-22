using Microsoft.AspNetCore.Http;

namespace BookingSystem.Application.DTOs
{
    public interface IResponseDto
    {
        bool Success { get; }
        string Message { get; }
        int StatusCode { get; }
    }
    public class ResponseDto<T>: IResponseDto
    {
        public bool Success { get; set; }    // Indicates if the operation was successful
        public string Message { get; set; }  // Success/Error message
        public T Data { get; set; }          // Actual response data
        public int StatusCode { get; set; }

        public static ResponseDto<T> SuccessResponse(T data,string message = "Operation completed successfully.")
        {
            return new ResponseDto<T>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ResponseDto<T> ErrorResponse(string message, int statusCode = StatusCodes.Status404NotFound)
        {
            return new ResponseDto<T>
            {
                StatusCode = statusCode,
                Success = false,
                Message = message,
                Data = default(T)
            };
        }
    }
}