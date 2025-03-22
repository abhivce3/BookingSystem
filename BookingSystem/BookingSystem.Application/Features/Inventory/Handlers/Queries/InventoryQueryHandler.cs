using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Features.Inventory.Queries;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using MediatR;

namespace BookingSystem.Application.Features.Inventory.Handlers.Queries
{
    public class InventoryQueryHandler : IRequestHandler<GetInventoryQuery, ResponseDto<InventoryDto>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public InventoryQueryHandler(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto<InventoryDto>> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
        {
                var inventory = await _inventoryRepository.GetInventoryByIdAsync(request.InventoryId);
                if (inventory == null)
                    return ResponseDto<InventoryDto>.ErrorResponse("Inventory not found");
                return ResponseDto<InventoryDto>.SuccessResponse(_mapper.Map<InventoryDto>(inventory));
        }
    }
}