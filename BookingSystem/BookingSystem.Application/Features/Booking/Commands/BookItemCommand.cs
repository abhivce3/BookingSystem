using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Booking.Commands
{
    public class BookItemCommand : IRequest<ResponseDto<BookingDto>>
    {
        public int MemberId { get; set; }
        public int InventoryId { get; set; }
    }
}