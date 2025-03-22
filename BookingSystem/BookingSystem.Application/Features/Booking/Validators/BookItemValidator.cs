using BookingSystem.Application.Features.Booking.Commands;
using FluentValidation;

namespace BookingSystem.Application.Features.Booking.Validators
{
    public class BookItemValidator : AbstractValidator<BookItemCommand>
    {
        public BookItemValidator()
        {
            RuleFor(x => x.MemberId)
                .GreaterThan(0).WithMessage("MemberId must be greater than zero.");

            RuleFor(x => x.InventoryId)
                .GreaterThan(0).WithMessage("InventoryId must be greater than zero.");
        }
    }
}