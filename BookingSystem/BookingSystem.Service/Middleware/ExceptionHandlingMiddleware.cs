using BookingSystem.Application.DTOs;
using Grpc.Core;
using System;
using System.Text.Json;

namespace BookingSystem.Service.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RpcException grpcEx)
            {
                // Handle gRPC-specific errors
                await HandleExceptionAsync(context, (int)grpcEx.Status.StatusCode, grpcEx.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {
            var errorResponse = new ResponseDto<string>
            {
                Success = false,
                Message = message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }

    // Extension method to use middleware
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}