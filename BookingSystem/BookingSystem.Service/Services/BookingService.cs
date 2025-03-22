using BookingSystem.Application.Features.Booking.Commands;
using BookingSystem.Application.Features.Booking.Queries;
using Grpc.Core;
using MediatR;
using BookingSystem.Service.Protos;
using Google.Protobuf.WellKnownTypes;

namespace BookingSystem.Service.Services
{
    public class BookingService : BookingGrpc.BookingGrpcBase
    {
        private readonly IMediator _mediator;

        public BookingService(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ Book an Item
        public override async Task<ResponseBooking> BookItem(BookItemRequest request, ServerCallContext context)
        {
            var command = new BookItemCommand
            {
                MemberId = request.MemberId,
                InventoryId = request.InventoryId
            };

            var result = await _mediator.Send(command);

            return new ResponseBooking
            {
                    Success = result.Success,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data != null ? new BookingDto
                    {
                        BookingId = result.Data.BookingId,
                        MemberId = result.Data.MemberId,
                        InventoryId = result.Data.InventoryId,
                        BookingDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(result.Data.BookingDate),
                    } : null
            };
        }

        // ✅ Cancel Booking
        public override async Task<ResponseBool> CancelBooking(CancelBookingRequest request, ServerCallContext context)
        {
            var command = new CancelBookingCommand { BookingId = request.BookingId };

            var result = await _mediator.Send(command);

            return new ResponseBool
            {
                    Success = result.Success,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data
            };
        }

        // ✅ Get Booking by ID
        public override async Task<ResponseBooking> GetBookingById(GetBookingByIdRequest request, ServerCallContext context)
        {
            var query = new GetBookingByIdQuery { BookingId = request.BookingId };

            var result = await _mediator.Send(query);

            return new ResponseBooking
            {
                    Success = result.Success,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data != null ? new BookingDto
                    {
                        BookingId = result.Data.BookingId,
                        MemberId = result.Data.MemberId,
                        InventoryId = result.Data.InventoryId,
                        BookingDate = Timestamp.FromDateTime(DateTime.SpecifyKind(result.Data.BookingDate, DateTimeKind.Utc)),
                    } : null
            };
        }

        // ✅ Get All Bookings
        public override async Task<ResponseBookingList> GetBookings(GetBookingsRequest request, ServerCallContext context)
        {
            var query = new GetBookingsQuery();
            var result = await _mediator.Send(query);

            var response = new ResponseBookingList
            {
                    Success = result.Success,
                    Message = result.Message,
                    StatusCode = result.StatusCode
            };

            if (result.Data != null)
            {
                response.Data.AddRange(result.Data.Select(b => new BookingDto
                {
                    BookingId = b.BookingId,
                    MemberId = b.MemberId,
                    InventoryId = b.InventoryId,
                    BookingDate = Timestamp.FromDateTime(DateTime.SpecifyKind(b.BookingDate, DateTimeKind.Utc)),
                }));
            }

            return response;
        }
    }
}
