using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Features.Member.Queries;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Member.Handlers.Queries
{
    public class MemberQueryHandler : IRequestHandler<GetMemberQuery, ResponseDto<MemberDto>>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public MemberQueryHandler(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto<MemberDto>> Handle(GetMemberQuery request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetMemberByIdAsync(request.MemberId);
            return member == null
                ? ResponseDto<MemberDto>.ErrorResponse("No Member found")
                : ResponseDto<MemberDto>.SuccessResponse(_mapper.Map<MemberDto>(member));
        }

    }
}