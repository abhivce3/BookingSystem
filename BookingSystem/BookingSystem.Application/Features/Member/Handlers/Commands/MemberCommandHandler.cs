using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Features.Member.Commands;
using BookingSystem.Application.Interfaces;
using BookingSystem.Application.Parsers;
using FluentValidation;
using MediatR;
using Entities = BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Features.Member.Handlers.Commands
{
    public class MemberCommandHandler : IRequestHandler<ImportMembersCommand, ResponseDto<bool>>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ImportMembersCommand> _importValidator;

        public MemberCommandHandler(IMemberRepository memberRepository, IMapper mapper, IValidator<ImportMembersCommand> importValidator)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
            _importValidator = importValidator;
        }

        public async Task<ResponseDto<bool>> Handle(ImportMembersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var memberDtos = await CsvParser.ParseCsvAsync<MemberDto>(request.FileByteArray, new MemberDtoMap());

                if (memberDtos == null || !memberDtos.Any())
                    return ResponseDto<bool>.ErrorResponse("CSV file is empty or invalid.");

                var memberEntities = _mapper.Map<List<Entities.Member>>(memberDtos);

                await _memberRepository.BulkInsertAsync(memberEntities);

                return ResponseDto<bool>.SuccessResponse(true, "Members imported successfully.");
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.ErrorResponse($"Error importing members: {ex.Message}");
            }
        }

    }
}