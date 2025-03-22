using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Inventory.Queries
{
    public class GetInventoryQuery : IRequest<ResponseDto<InventoryDto>>
    {
        public int InventoryId { get; set; }
    }
}