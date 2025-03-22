using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Features.Inventory.Commands;
using BookingSystem.Application.Interfaces;
using BookingSystem.Application.Parsers;
using FluentValidation;
using MediatR;
using Entities = BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Features.Inventory.Handlers.Commands
{
    public class InventoryCommandHandler : IRequestHandler<ImportInventoryCommand, ResponseDto<bool>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ImportInventoryCommand> _importValidator;

        public InventoryCommandHandler(IInventoryRepository inventoryRepository, IMapper mapper, IValidator<ImportInventoryCommand> importValidator)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
            _importValidator = importValidator;
        }

        public async Task<ResponseDto<bool>> Handle(ImportInventoryCommand request, CancellationToken cancellationToken)
        {
            var inventoryDtos = await CsvParser.ParseCsvAsync<InventoryDto>(request.FileByteArray, new InventoryDtoMap());
                var inventoryEntities = _mapper.Map<List<Entities.Inventory>>(inventoryDtos);
                await _inventoryRepository.BulkInsertAsync(inventoryEntities);
                return ResponseDto<bool>.SuccessResponse(true, "Inventory imported successfully.");
        }
    }
}