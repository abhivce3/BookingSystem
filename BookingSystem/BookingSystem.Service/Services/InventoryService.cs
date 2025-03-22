using BookingSystem.Application.Features.Inventory.Commands;
using BookingSystem.Application.Features.Inventory.Queries;
using BookingSystem.Service.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

namespace BookingSystem.Service.Services
{
    public class InventoryService : InventoryGrpc.InventoryGrpcBase
    {
        private readonly IMediator _mediator;

        public InventoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ Import Inventory
        public override async Task<ResponseBool> ImportInventory(ImportInventoryRequest request, ServerCallContext context)
        {

            var command = new ImportInventoryCommand(request.FileData.ToByteArray());

            var result = await _mediator.Send(command);

            return new ResponseBool
            {
                Success = result.Success,
                Message = result.Message,
                StatusCode = result.StatusCode,
                Data = result.Data
            };
        }


        // ✅ Get Inventory by ID
        public override async Task<ResponseInventory> GetInventoryById(GetInventoryByIdRequest request, ServerCallContext context)
        {
            var query = new GetInventoryQuery { InventoryId = request.InventoryId };
            var result = await _mediator.Send(query);

            return new ResponseInventory
            {
                Success = result.Success,
                Message = result.Message,
                StatusCode = result.StatusCode,
                Data = result.Data != null ? new InventoryDto
                {
                    InventoryId = result.Data.InventoryId,
                    Title = result.Data.Title,
                    RemainingCount = result.Data.RemainingCount,
                    Description = result.Data.Description,
                    ExpirationDate = Timestamp.FromDateTime(DateTime.SpecifyKind(result.Data.ExpirationDate, DateTimeKind.Utc))
                } : null
            };
        }
    }
}
