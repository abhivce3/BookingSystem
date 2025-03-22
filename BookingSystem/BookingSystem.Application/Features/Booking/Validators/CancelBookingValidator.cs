using BookingSystem.Application.Features.Booking.Commands;
using FluentValidation;

namespace BookingSystem.Application.Features.Booking.Validators
{
    public class CancelBookingValidator : AbstractValidator<CancelBookingCommand>
    {
        public CancelBookingValidator()
        {
            RuleFor(x => x.BookingId)
                .GreaterThan(0).WithMessage("BookingId must be greater than zero.");
        }
    }
}