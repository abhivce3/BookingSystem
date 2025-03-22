using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Inventory.Commands
{
    public class ImportInventoryCommand : IRequest<ResponseDto<bool>>
    {
        public byte[] FileByteArray { get; }

        public ImportInventoryCommand(byte[] fileByteArray)
        {
            FileByteArray = fileByteArray;
        }
        public ImportInventoryCommand() { } // ✅ Required for serialization

    }

}