using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<bool> BookItem(Booking booking);

        Task<bool> CancelBooking(int bookingId);

        Task<Booking> GetBookingByIdAsync(int bookingId);

        Task<List<Booking>> GetAllBookingsAsync();
    }
}