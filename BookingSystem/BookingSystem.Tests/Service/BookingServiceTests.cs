using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Features.Booking.Commands;
using BookingSystem.Application.Features.Booking.Queries;
using BookingSystem.Service.Protos;
using BookingSystem.Service.Services;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Moq;
using Xunit;

namespace BookingSystem.Tests.Service
{
    public class BookingServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BookingService _bookingService;
        private readonly Fixture _fixture;
        private readonly ServerCallContext _serverCallContext;

        public BookingServiceTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _bookingService = new BookingService(_mediatorMock.Object);
            _fixture = new Fixture();
            _serverCallContext = TestServerCallContext.Create();
        }

        // ✅ Test: Successful Booking
        [Fact]
        public async Task BookItem_ShouldReturnSuccess_WhenBookingSucceeds()
        {
            // Arrange
            var request = new BookItemRequest { MemberId = 1, InventoryId = 100 };
            var expectedResponse = new ResponseDto<Application.DTOs.BookingDto>
            {
                Success = true,
                Message = "Booking successful",
                StatusCode = 200,
                Data = new Application.DTOs.BookingDto
                {
                    BookingId = 1,
                    MemberId = request.MemberId,
                    InventoryId = request.InventoryId,
                    BookingDate = DateTime.UtcNow
                }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<BookItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _bookingService.BookItem(request, _serverCallContext);

            // Assert
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Booking successful");
            response.Data.Should().NotBeNull();
            response.Data.MemberId.Should().Be(request.MemberId);
            response.Data.InventoryId.Should().Be(request.InventoryId);
        }

        // ✅ Test: Booking Fails Due to Unavailable Inventory
        [Fact]
        public async Task BookItem_ShouldReturnFailure_WhenInventoryNotAvailable()
        {
            // Arrange
            var request = new BookItemRequest { MemberId = 1, InventoryId = 100 };

            var expectedResponse = ResponseDto<Application.DTOs.BookingDto>.ErrorResponse("Inventory not available.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<BookItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _bookingService.BookItem(request, _serverCallContext);

            // Assert
            response.Success.Should().BeFalse();
            response.Message.Should().Be("Inventory not available.");
            response.Data.Should().BeNull();
        }

        // ✅ Test: Successful Cancellation
        [Fact]
        public async Task CancelBooking_ShouldReturnSuccess_WhenCancellationSucceeds()
        {
            // Arrange
            var request = new CancelBookingRequest { BookingId = 10 };
            var expectedResponse = ResponseDto<bool>.SuccessResponse(true, "Booking cancelled successfully.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CancelBookingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _bookingService.CancelBooking(request, _serverCallContext);

            // Assert
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Booking cancelled successfully.");
            response.Data.Should().BeTrue();
        }

        // ✅ Test: Cancellation Fails When Booking Not Found
        [Fact]
        public async Task CancelBooking_ShouldReturnFailure_WhenBookingNotFound()
        {
            // Arrange
            var request = new CancelBookingRequest { BookingId = 999 };

            var expectedResponse = ResponseDto<bool>.ErrorResponse("Booking not found.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CancelBookingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _bookingService.CancelBooking(request, _serverCallContext);

            // Assert
            response.Success.Should().BeFalse();
            response.Message.Should().Be("Booking not found.");
            response.Data.Should().BeFalse();
        }

        // ✅ Test: Get Booking By ID - Success
        [Fact]
        public async Task GetBookingById_ShouldReturnBooking_WhenBookingExists()
        {
            // Arrange
            var request = new GetBookingByIdRequest { BookingId = 1 };

            var expectedBooking = _fixture.Build<Application.DTOs.BookingDto>()
                .With(b => b.BookingDate, DateTime.UtcNow) // ✅ Ensures UTC DateTime
                .Create();

            var expectedResponse = ResponseDto<Application.DTOs.BookingDto>.SuccessResponse(expectedBooking);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBookingByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _bookingService.GetBookingById(request, _serverCallContext);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.BookingId.Should().Be(expectedBooking.BookingId);
        }


        // ✅ Test: Get Booking By ID - Not Found
        [Fact]
        public async Task GetBookingById_ShouldReturnError_WhenBookingNotFound()
        {
            // Arrange
            var request = new GetBookingByIdRequest { BookingId = 999 };

            var expectedResponse = ResponseDto<Application.DTOs.BookingDto>.ErrorResponse("Booking not found.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBookingByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _bookingService.GetBookingById(request, _serverCallContext);

            // Assert
            response.Success.Should().BeFalse();
            response.Message.Should().Be("Booking not found.");
            response.Data.Should().BeNull();
        }

        // ✅ Test: Get All Bookings
        [Fact]
        public async Task GetBookings_ShouldReturnBookingList_WhenBookingsExist()
        {
            // Arrange
            var request = new GetBookingsRequest();

            var bookings = _fixture.CreateMany<Application.DTOs.BookingDto>(5)
                .Select(b => new Application.DTOs.BookingDto
                {
                    BookingId = b.BookingId,
                    MemberId = b.MemberId,
                    InventoryId = b.InventoryId,
                    BookingDate = DateTime.UtcNow // ✅ Ensure UTC DateTime
                }).ToList();

            var expectedResponse = ResponseDto<List<Application.DTOs.BookingDto>>.SuccessResponse(bookings);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBookingsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _bookingService.GetBookings(request, _serverCallContext);

            // Assert
            response.Success.Should().BeTrue();
            response.Data.Should().NotBeEmpty();
            response.Data.Should().HaveCount(5);
        }


        // ✅ Test: Get All Bookings - No Data
        [Fact]
        public async Task GetBookings_ShouldReturnEmpty_WhenNoBookingsExist()
        {
            // Arrange
            var request = new GetBookingsRequest();

            var expectedResponse = ResponseDto<List<Application.DTOs.BookingDto>>.ErrorResponse("No bookings found.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBookingsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _bookingService.GetBookings(request, _serverCallContext);

            // Assert
            response.Success.Should().BeFalse();
            response.Message.Should().Be("No bookings found.");
            response.Data.Should().BeEmpty();
        }
    }
}
