using AutoMapper;
using BookingSystem.Application.Config;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Features.Booking.Commands;
using BookingSystem.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Entities = BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Features.Booking.Handlers.Commands
{
    public class BookingCommandHandler : IRequestHandler<BookItemCommand, ResponseDto<BookingDto>>,
                                          IRequestHandler<CancelBookingCommand, ResponseDto<bool>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IValidator<BookItemCommand> _bookItemValidator;
        private readonly IValidator<CancelBookingCommand> _cancelBookingValidator;
        private readonly IMapper _mapper;
        private readonly BookingConfig _bookingConfig;

        public BookingCommandHandler(
            IBookingRepository bookingRepository,
            IInventoryRepository inventoryRepository,
            IMemberRepository memberRepository,
            IValidator<BookItemCommand> bookItemValidator,
            IValidator<CancelBookingCommand> cancelBookingValidator,
            IMapper mapper,
            IOptions<BookingConfig> bookingConfig
            )
        {
            _bookingRepository = bookingRepository;
            _inventoryRepository = inventoryRepository;
            _memberRepository = memberRepository;
            _bookItemValidator = bookItemValidator;
            _cancelBookingValidator = cancelBookingValidator;
            _mapper = mapper;
            _bookingConfig = bookingConfig.Value; // ✅ Get max bookings value
        }

        public async Task<ResponseDto<BookingDto>> Handle(BookItemCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetMemberByIdAsync(request.MemberId);
            if (member == null)
                return ResponseDto<BookingDto>.ErrorResponse("Member not found.");
            if (member.Bookings.Count >= _bookingConfig.MaxBookings)
                return ResponseDto<BookingDto>.ErrorResponse($"Can not book more than allowed bookings: {_bookingConfig.MaxBookings}.");

            var inventory = await _inventoryRepository.GetInventoryByIdAsync(request.InventoryId);
            if (inventory == null || inventory.RemainingCount <= 0)
                return ResponseDto<BookingDto>.ErrorResponse("Inventory not available for booking.");
            else if (inventory.ExpirationDate < DateTime.UtcNow)
                return ResponseDto<BookingDto>.ErrorResponse("Inventory is expired.");

            var booking = new Entities.Booking
            {
                MemberId = request.MemberId,
                InventoryId = request.InventoryId,
                BookingDate = DateTime.UtcNow
            };

            var success = await _bookingRepository.BookItem(booking);

            if (!success)
                return ResponseDto<BookingDto>.ErrorResponse("Booking failed.");

            return ResponseDto<BookingDto>.SuccessResponse(_mapper.Map<BookingDto>(booking)); ;
        }

        public async Task<ResponseDto<bool>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId);
            if (booking == null)
                return ResponseDto<bool>.ErrorResponse("Booking not found.");

            var success = await _bookingRepository.CancelBooking(request.BookingId);

            if (!success)
                return ResponseDto<bool>.ErrorResponse("Cancellation failed.");

            return ResponseDto<bool>.SuccessResponse(true, "Booking cancelled successfully.");
        }
    }
}