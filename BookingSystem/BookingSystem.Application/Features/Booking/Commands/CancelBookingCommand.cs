using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Booking.Commands
{
    public class CancelBookingCommand : IRequest<ResponseDto<bool>>
    {
        public int BookingId { get; set; }
    }
}