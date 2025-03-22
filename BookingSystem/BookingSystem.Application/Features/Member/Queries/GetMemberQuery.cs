using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Member.Queries
{
    public class GetMemberQuery : IRequest<ResponseDto<MemberDto>>
    {
        public int MemberId { get; set; }
    }
}