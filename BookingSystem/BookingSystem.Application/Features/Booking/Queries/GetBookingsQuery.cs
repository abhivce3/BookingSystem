using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Booking.Queries
{
    public class GetBookingsQuery : IRequest<ResponseDto<List<BookingDto>>>
    { }
}