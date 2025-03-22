using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Features.Booking.Queries;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Booking.Handlers.Queries
{
    public class BookingQueryHandler : IRequestHandler<GetBookingByIdQuery, ResponseDto<BookingDto>>,
                                       IRequestHandler<GetBookingsQuery, ResponseDto<List<BookingDto>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto<BookingDto>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId);
            if (booking == null)
                return ResponseDto<BookingDto>.ErrorResponse("Booking not found");
            return ResponseDto<BookingDto>.SuccessResponse(_mapper.Map<BookingDto>(booking));

        }

        public async Task<ResponseDto<List<BookingDto>>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();
            if (bookings == null || bookings.Count <= 0)
                return ResponseDto<List<BookingDto>>.ErrorResponse("Bookings not found");
            return ResponseDto<List<BookingDto>>.SuccessResponse(_mapper.Map<List<BookingDto>>(bookings));

        }
    }
}