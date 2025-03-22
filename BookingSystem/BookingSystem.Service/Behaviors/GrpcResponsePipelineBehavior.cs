using BookingSystem.Application.DTOs;
using Grpc.Core;
using MediatR;

namespace BookingSystem.Service.Behaviors
{
    public class GrpcResponsePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TResponse : class
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await next();

                if (response is IResponseDto responseDto)
                {
                    if (!responseDto.Success)
                    {
                        var grpcStatus = MapToGrpcStatus(responseDto.StatusCode);
                        var metadata = new Metadata { { "response-message", responseDto.Message } };
                        throw new RpcException(new Status(grpcStatus, responseDto.Message), metadata);
                    }
                    return response;
                }

                return response;
            }
            catch (RpcException)  // ✅ If it's already an RpcException, let it propagate
            {
                throw; // ✅ Re-throw the existing gRPC exception without modification
            }
            catch (Exception ex)
            {
                var metadata = new Metadata { { "response-message", "An unexpected error occurred." } };
                throw new RpcException(new Status(StatusCode.Internal, ex.Message), metadata);
            }
        }

        private StatusCode MapToGrpcStatus(int httpStatusCode)
        {
            return httpStatusCode switch
            {
                StatusCodes.Status200OK => StatusCode.OK,
                StatusCodes.Status400BadRequest => StatusCode.InvalidArgument,
                StatusCodes.Status401Unauthorized => StatusCode.Unauthenticated,
                StatusCodes.Status403Forbidden => StatusCode.PermissionDenied,
                StatusCodes.Status404NotFound => StatusCode.NotFound,
                StatusCodes.Status409Conflict => StatusCode.AlreadyExists,
                StatusCodes.Status500InternalServerError => StatusCode.Internal,
                _ => StatusCode.Unknown
            };
        }
    }
}
