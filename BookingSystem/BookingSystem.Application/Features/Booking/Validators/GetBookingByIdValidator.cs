using BookingSystem.Application.Features.Booking.Commands;
using BookingSystem.Application.Features.Booking.Queries;
using FluentValidation;

namespace BookingSystem.Application.Features.Booking.Validators
{
    public class GetBookingByIdValidator : AbstractValidator<GetBookingByIdQuery>
    {
        public GetBookingByIdValidator()
        {
            RuleFor(x => x.BookingId)
            .GreaterThan(0).WithMessage("BookingId must be greater than zero.");
        }
    }
}