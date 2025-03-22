using BookingSystem.Application.Features.Member.Commands;
using BookingSystem.Application.Features.Member.Queries;
using BookingSystem.Service.Protos;
using Grpc.Core;
using MediatR;
using Google.Protobuf.WellKnownTypes;

namespace BookingSystem.Service.Services
{
    public class MemberService : MemberGrpc.MemberGrpcBase
    {
        private readonly IMediator _mediator;

        public MemberService(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ Import Members
        public override async Task<ResponseBool> ImportMembers(ImportMembersRequest request, ServerCallContext context)
        {
            try
            {
                var command = new ImportMembersCommand(request.FileData.ToByteArray());
                var result = await _mediator.Send(command);

                return new ResponseBool
                {
                    Success = result.Success,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data
                };
            }
            catch (Exception ex)
            {
                return new ResponseBool
                {
                    Success = false,
                    Message = $"Error importing members: {ex.Message}",
                    StatusCode = 500,
                    Data = false
                };
            }
        }

        // ✅ Get Member by ID
        public override async Task<ResponseMember> GetMemberById(GetMemberByIdRequest request, ServerCallContext context)
        {
            var query = new GetMemberQuery { MemberId = request.MemberId };
            var result = await _mediator.Send(query);

            return new ResponseMember
            {
                Success = result.Success,
                Message = result.Message,
                StatusCode = result.StatusCode,
                Data = result.Data != null ? new MemberDto
                {
                    MemberId = result.Data.MemberId,
                    FirstName = result.Data.FirstName,
                    LastName = result.Data.LastName,
                    BookingCount = result.Data.BookingCount,
                    DateJoined = Timestamp.FromDateTime(DateTime.SpecifyKind(result.Data.DateJoined, DateTimeKind.Utc))
                } : null
            };
        }
    }
}
