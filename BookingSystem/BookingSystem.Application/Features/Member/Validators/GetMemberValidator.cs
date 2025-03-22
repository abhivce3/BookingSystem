using BookingSystem.Application.Features.Member.Queries;
using FluentValidation;

namespace BookingSystem.Application.Features.Member.Validators
{
    public class GetMemberValidator : AbstractValidator<GetMemberQuery>
    {
        public GetMemberValidator()
        {
            RuleFor(x => x.MemberId)
                .GreaterThan(0).WithMessage("MemberId must be greater than zero.");
        }
    }
}