using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DatabaseContext _context;

        public BookingRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> BookItem(Booking booking)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var inventory = await _context.Inventories.FindAsync(booking.InventoryId);
                var member = await _context.Members
                            .Include(m => m.Bookings) // ✅ Includes the related Bookings
                            .FirstOrDefaultAsync(m => m.MemberId == booking.MemberId);

                if (inventory == null || member == null ||
                    inventory.RemainingCount <= 0 ||
                    inventory.ExpirationDate < DateTime.UtcNow)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                if (member.BookingCount > member.Bookings.Count)
                {
                    member.BookingCount = 1;
                }
                else
                {
                    member.BookingCount += 1;

                }


                inventory.RemainingCount -= 1;  // Reduce inventory count

                await _context.Bookings.AddAsync(booking);


                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CancelBooking(int bookingId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null) return false;
                booking.IsCancelled = true;

                var inventory = await _context.Inventories.FindAsync(booking.InventoryId);

                var member = await _context.Members.FindAsync(booking.MemberId);

                if (inventory == null || member == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                inventory.RemainingCount += 1;
                member.BookingCount -= 1;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            try
            {
                return await _context.Bookings
                    .AsNoTracking()
                    .Include(b => b.Member)
                    .Include(b => b.Inventory)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId && !b.IsCancelled);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            try
            {
                return await _context.Bookings
                    .Where(b=> !b.IsCancelled)
                    .Include(b => b.Member)
                    .Include(b => b.Inventory)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}