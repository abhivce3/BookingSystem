using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Booking.Queries
{
    public class GetBookingByIdQuery : IRequest<ResponseDto<BookingDto>>
    {
        public int BookingId { get; set; }
    }
}