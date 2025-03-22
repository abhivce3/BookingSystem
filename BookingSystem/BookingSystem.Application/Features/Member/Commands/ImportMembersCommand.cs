using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Member.Commands
{
    public class ImportMembersCommand : IRequest<ResponseDto<bool>>
    {
        public byte[] FileByteArray { get; }

        public ImportMembersCommand(byte[] fileByteArray)
        {
            FileByteArray = fileByteArray ?? throw new ArgumentNullException(nameof(fileByteArray));
        }

        public ImportMembersCommand() { } // ✅ Required for serialization
    }

}