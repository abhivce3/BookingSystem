using BookingSystem.Application.Features.Inventory.Queries;
using FluentValidation;

namespace BookingSystem.Application.Features.Inventory.Validators
{
    public class GetInventoryValidator : AbstractValidator<GetInventoryQuery>
    {
        public GetInventoryValidator()
        {
            RuleFor(x => x.InventoryId)
                .GreaterThan(0).WithMessage("InventoryId must be greater than zero.");
        }
    }
}