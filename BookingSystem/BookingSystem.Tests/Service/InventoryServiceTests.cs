namespace BookingSystem.Tests.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using BookingSystem.Application.DTOs;
    using BookingSystem.Application.Features.Inventory.Commands;
    using BookingSystem.Application.Features.Inventory.Queries;
    using BookingSystem.Service.Protos;
    using BookingSystem.Service.Services;
    using FluentAssertions;
    using Google.Protobuf;
    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using MediatR;
    using Moq;
    using Xunit;

    public class InventoryServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly InventoryService _inventoryService;
        private readonly Fixture _fixture;
        private readonly ServerCallContext _serverCallContext;

        public InventoryServiceTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _inventoryService = new InventoryService(_mediatorMock.Object);
            _fixture = new Fixture();
            _serverCallContext = TestServerCallContext.Create();
        }

        // ✅ Test: Import Inventory Successfully
        [Fact]
        public async Task ImportInventory_ShouldReturnSuccess_WhenImportSucceeds()
        {
            // Arrange
            var request = new ImportInventoryRequest { FileData = ByteString.CopyFrom(new byte[] { 1, 2, 3 }) };
            var expectedResponse = new ResponseDto<bool>
            {
                Success = true,
                Message = "Inventory imported successfully",
                StatusCode = 200,
                Data = true
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ImportInventoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _inventoryService.ImportInventory(request, _serverCallContext);

            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Inventory imported successfully");
            response.Data.Should().BeTrue();
        }

        // ✅ Test: Import Inventory Failure Due to Invalid Data
        [Fact]
        public async Task ImportInventory_ShouldReturnFailure_WhenImportFails()
        {
            // Arrange
            var request = new ImportInventoryRequest { FileData = ByteString.Empty };

            var expectedResponse = ResponseDto<bool>.ErrorResponse("Invalid file data");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ImportInventoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _inventoryService.ImportInventory(request, _serverCallContext);

            // Assert
            response.Success.Should().BeFalse();
            response.Message.Should().Be("Invalid file data");
            response.Data.Should().BeFalse();
        }

        // ✅ Test: Get Inventory By ID - Success
        [Fact]
        public async Task GetInventoryById_ShouldReturnInventory_WhenInventoryExists()
        {
            // Arrange
            var request = new GetInventoryByIdRequest { InventoryId = 1 };

            var expectedInventory = _fixture.Build<Application.DTOs.InventoryDto>()
                .With(i => i.ExpirationDate, DateTime.UtcNow) // ✅ Ensures UTC DateTime
                .Create();

            var expectedResponse = ResponseDto<Application.DTOs.InventoryDto>.SuccessResponse(expectedInventory);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInventoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _inventoryService.GetInventoryById(request, _serverCallContext);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.InventoryId.Should().Be(expectedInventory.InventoryId);
            response.Data.ExpirationDate.Should().NotBeNull();
        }

        // ✅ Test: Get Inventory By ID - Not Found
        [Fact]
        public async Task GetInventoryById_ShouldReturnError_WhenInventoryNotFound()
        {
            // Arrange
            var request = new GetInventoryByIdRequest { InventoryId = 999 };

            var expectedResponse = ResponseDto<Application.DTOs.InventoryDto>.ErrorResponse("Inventory not found.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInventoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _inventoryService.GetInventoryById(request, _serverCallContext);

            // Assert
            response.Success.Should().BeFalse();
            response.Message.Should().Be("Inventory not found.");
            response.Data.Should().BeNull();
        }
    }

}
