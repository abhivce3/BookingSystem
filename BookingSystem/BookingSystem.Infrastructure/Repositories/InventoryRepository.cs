using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Persistence;

namespace BookingSystem.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DatabaseContext _context;

        public InventoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            return await _context.Inventories.FindAsync(inventoryId);
        }

        public async Task BulkInsertAsync(List<Inventory> inventories)
        {
            await _context.Inventories.AddRangeAsync(inventories);
            await _context.SaveChangesAsync();
        }
    }
}